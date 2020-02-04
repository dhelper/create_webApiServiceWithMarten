using System;
using System.Linq;
using System.Reflection;
using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UsersService.Controllers;
using UsersService.DataAccess;
using UsersService.Models;

namespace UsersService.Tests
{
    [TestClass]
    public class UsersControllerTests
    {
        [TestMethod]
        public void Task2_CreateNewUserShouldCallUserRepository()
        {
            const string userName = "user name";
            const string email = "user@email.com";
            const int age = 42;

            using var fake = new AutoFake();

            var usersController = fake.Resolve<UsersController>();
            
            var userCreateData = new UserCreate
            {
                Name = userName,
                Email = email,
                Age = age
            };

            usersController.CreateNewUser(userCreateData);

            var fakeUsersRepository = fake.Resolve<IUsersRepository>();

            var foundCall = TestUtilities.GetMethodCall(() => fakeUsersRepository.CreateNewUser("", "", 0));

            Assert.IsNotNull(foundCall, CreateNewUserNotCalledError);

            Assert.That.All(
                () => Assert.AreEqual(userName, foundCall.Get<string>("name"), UserNameNotPassedError),
                () => Assert.AreEqual(email, foundCall.Get<string>("email"), UserEmailNotPassedError),
                () => Assert.AreEqual(42, foundCall.Get<int>("age"), UserAgeNotPassedError)
                );
        }

        [TestMethod]
        public void Task2_CreateNewUserShouldReturnCreatedUser()
        {
            using var fake = new AutoFake();

            var fakeUsersRepository = fake.Resolve<IUsersRepository>();
            A.CallTo(() => fakeUsersRepository.CreateNewUser(A<String>._, A<string>._, A<int>._))
                .Returns("user-1");

            var usersController = fake.Resolve<UsersController>();

            var actual = usersController.CreateNewUser(new UserCreate());

            Assert.AreSame("user-1", actual, UserIdNotReturnedError);
        }

        #region errors

        private readonly string CreateNewUserNotCalledError =
            @"
Error: UsersRepository.CreateNewUser was not called 
---------------------------------------------------
    Expected UsersRepository.CreateNewUser(...) to be called as part of user creation
    Add the following line to UsersController.CreateNewUser:

    public string CreateNewUser([FromBody] UserCreate userCreateData)
    {
        _usersRepository.CreateNewUser(""name"", ""email"", 1234);    
    
";


        private readonly string UserNameNotPassedError =
            @"
Error: UsersRepository.CreateNewUser did not receive user's name 
----------------------------------------------------------------
    Make sure that the value passed to CreateNewUser method is used when calling usersRepository
    
    Example:

    public string CreateNewUser([FromBody] UserCreate userCreateData)
    {
        _usersRepository.CreateNewUser(userCreateData.Name, ..., ...);    
    
";

        private readonly string UserEmailNotPassedError =
            @"
Error: UsersRepository.CreateNewUser did not receive user's email 
-----------------------------------------------------------------
    Make sure that the value passed to CreateNewUser method is used when calling usersRepository
    
    Example:

    public string CreateNewUser([FromBody] UserCreate userCreateData)
    {
        _usersRepository.CreateNewUser(..., userCreateData.Email, ...);    
    
";

        private readonly string UserAgeNotPassedError =
            @"
Error: UsersRepository.CreateNewUser did not receive user's age 
---------------------------------------------------------------
    Make sure that the value passed to CreateNewUser method is used when calling usersRepository
    
    Example:

    public string CreateNewUser([FromBody] UserCreate userCreateData)
    {
        _usersRepository.CreateNewUser(..., ..., userCreateData.Age);    
    
";

        private readonly string UserIdNotReturnedError =
            @"
Error: Created user's id not returned from method call
------------------------------------------------------
    Make sure to return the value returned from usersRepository after a new user was created 

    Example:

    public string CreateNewUser([FromBody] UserCreate userCreateData)
    {
        var id = _usersRepository.CreateNewUser(..., ..., ...);    

        return id;
";

        #endregion
    }
}
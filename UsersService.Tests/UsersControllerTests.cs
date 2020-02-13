using System;
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
        public void Task2_CreateNewUserShouldReturnCreatedUserId()
        {
            using var fake = new AutoFake();

            var fakeUsersRepository = fake.Resolve<IUsersRepository>();
            A.CallTo(() => fakeUsersRepository.CreateNewUser(A<String>._, A<string>._, A<int>._))
                .Returns(123);

            var usersController = fake.Resolve<UsersController>();

            var actual = usersController.CreateNewUser(new UserCreate());

            Assert.AreEqual(123, actual, UserIdNotReturnedError);
        }

        [TestMethod]
        public void Task4_GetUserByIdShouldCallDataAccess()
        {
            using var fake = new AutoFake();

            var fakeUsersRepository = fake.Resolve<IUsersRepository>();
            var user = new User
            {
                Id = 1,
                Name = "user1",
                Age = 42,
                Email = "user1@email.com"
            };

            A.CallTo(() => fakeUsersRepository.GetUserById(1))
                .Returns(user);

            var usersController = fake.Resolve<UsersController>();

            var actual = usersController.GetUserById(1);

            Assert.IsNotNull(actual, GetUserByIdReturnsNullError);
            Assert.AreEqual(user, actual, GetUserByIdReturnsWrongUserError);
        }

        [TestMethod]
        public void Task5_DeleteUserShouldCallUsersRepository()
        {
            using var fake = new AutoFake();

            var usersController = fake.Resolve<UsersController>();
            usersController.Delete(100);

            var fakeUsersRepository = fake.Resolve<IUsersRepository>();

            var fakeObjectCall = TestUtilities.GetMethodCall(() => fakeUsersRepository.DeleteUser(0));

            Assert.IsNotNull(fakeObjectCall, DeleteUserWasNotCalledError);
            Assert.AreEqual(100, fakeObjectCall.Arguments.Get<int>(0), DeleteCalledWithWrongIdError);
        }

        #region errors

        private readonly string CreateNewUserNotCalledError =
            @"
Error: UsersRepository.CreateNewUser was not called 
---------------------------------------------------
    Expected UsersRepository.CreateNewUser(...) to be called as part of user creation
    Add the following line to UsersController.CreateNewUser:

    public int CreateNewUser([FromBody] UserCreate userCreateData)
    {
        _usersRepository.CreateNewUser(""name"", ""email"", 1234);    
    
";

        private readonly string DeleteUserWasNotCalledError =
            @"
Error: UsersRepository.DeleteUser was not called 
------------------------------------------------
    Expected UsersRepository.DeleteUser(id) to be called
    DeleteUser should look like the following example:

    public void Delete(int id)
    {
        _usersRepository.DeleteUser(id);
    }
";  


        private readonly string UserNameNotPassedError =
            @"
Error: UsersRepository.CreateNewUser did not receive user's name 
----------------------------------------------------------------
    Make sure that the value passed to CreateNewUser method is used when calling usersRepository
    
    Example:

    public int CreateNewUser([FromBody] UserCreate userCreateData)
    {
        _usersRepository.CreateNewUser(userCreateData.Name, ..., ...);    
    
";

        private readonly string UserEmailNotPassedError =
            @"
Error: UsersRepository.CreateNewUser did not receive user's email 
-----------------------------------------------------------------
    Make sure that the value passed to CreateNewUser method is used when calling usersRepository
    
    Example:

    public int CreateNewUser([FromBody] UserCreate userCreateData)
    {
        _usersRepository.CreateNewUser(..., userCreateData.Email, ...);    
    
";

        private readonly string UserAgeNotPassedError =
            @"
Error: UsersRepository.CreateNewUser did not receive user's age 
---------------------------------------------------------------
    Make sure that the value passed to CreateNewUser method is used when calling usersRepository
    
    Example:

    public int CreateNewUser([FromBody] UserCreate userCreateData)
    {
        _usersRepository.CreateNewUser(..., ..., userCreateData.Age);    
    
";

        private readonly string UserIdNotReturnedError =
            @"
Error: Created user's id not returned from method call
------------------------------------------------------
    Make sure to return the value returned from usersRepository after a new user was created 

    Example:

    public int CreateNewUser([FromBody] UserCreate userCreateData)
    {
        var id = _usersRepository.CreateNewUser(..., ..., ...);    

        return id;
";

        private readonly string GetUserByIdReturnsNullError =
            @"
Error: GetUserById returned null 
--------------------------------
    GetUserById returned null, this might mean you did not pass the correct id to the UsersRepository.

    Try this:

    public User GetUserById(int id)
    {
        return _usersRepository.GetUserById(id);    
    }    
";

        private readonly string GetUserByIdReturnsWrongUserError =
            @"
Error: GetUserById returned the wrong user 
------------------------------------------
    GetUserById returned wrong user value, make sure you return the value from UsersRepository

    Try this:

    public User GetUserById(int id)
    {
        return _usersRepository.GetUserById(id);    
    }    
";

        private readonly string DeleteCalledWithWrongIdError =
            @"
Error: Delete was called with the wrong user id
-----------------------------------------------
    UsersRepository.Delete was called with the wrong user id
    Make sure you pass the same id from the controller to the UsersRepository

    Try this:

    [HttpDelete(""{id}"")]
    public void Delete(int id)
    {
        _usersRepository.DeleteUser(id);
    
    }
";
        #endregion
    }
}
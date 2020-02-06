using System.Data;
using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using Marten;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UsersService.DataAccess;
using UsersService.Models;

namespace UsersService.Tests
{
    [TestClass]
    public class UsersRepositoryTests
    {
        [TestMethod]
        public void Task3_CreateNewUserShouldCallOpenSession()
        {
            using var autoFake = new AutoFake();

            var fakeDocumentStoreFactory = autoFake.Resolve<IDocumentStoreFactory>();
            var fakeDocumentStore = A.Fake<IDocumentStore>();
            A.CallTo(() => fakeDocumentStoreFactory.Store).Returns(fakeDocumentStore);

            var usersRepository = autoFake.Resolve<UsersRepository>();

            usersRepository.CreateNewUser("", "", 0);


            var wasCalled = TestUtilities.WasCalled(() => fakeDocumentStore.OpenSession(DocumentTracking.None, IsolationLevel.Unspecified));

            Assert.IsTrue(wasCalled, OpenSessionNotCalledError);
        }


        [TestMethod]
        public void Task3_CreateNewUserShouldCallSaveChanges()
        {
            using var autoFake = new AutoFake();

            var fakeDocumentStoreFactory = autoFake.Resolve<IDocumentStoreFactory>();
            var fakeDocumentStore = A.Fake<IDocumentStore>();
            A.CallTo(() => fakeDocumentStoreFactory.Store).Returns(fakeDocumentStore);
            var fakeDocumentSession = A.Fake<IDocumentSession>();
            A.CallTo(() => fakeDocumentStore.OpenSession(A<DocumentTracking>._, A<IsolationLevel>._))
                .Returns(fakeDocumentSession);

            var usersRepository = autoFake.Resolve<UsersRepository>();

            usersRepository.CreateNewUser("", "", 0);

            var wasCalled = TestUtilities.WasCalled(() => fakeDocumentSession.SaveChanges());

            Assert.IsTrue(wasCalled, SaveChangesNotCalledError);
        }

        [TestMethod]
        public void Task3_CreateNewUserShouldCallStoreWitUserData()
        {
            using var autoFake = new AutoFake();

            var fakeDocumentStoreFactory = autoFake.Resolve<IDocumentStoreFactory>();
            var fakeDocumentStore = A.Fake<IDocumentStore>();
            A.CallTo(() => fakeDocumentStoreFactory.Store).Returns(fakeDocumentStore);
            var fakeDocumentSession = A.Fake<IDocumentSession>();
            A.CallTo(() => fakeDocumentStore.OpenSession(A<DocumentTracking>._, A<IsolationLevel>._))
                .Returns(fakeDocumentSession);

            var usersRepository = autoFake.Resolve<UsersRepository>();

            usersRepository.CreateNewUser("user name", "user@email.com", 42);

            var methodCall = TestUtilities.GetMethodCall(() => fakeDocumentSession.Store(new User[0]));

            Assert.IsNotNull(methodCall, StoreNotCalledError);

            var users = methodCall.Get<User[]>("entities");

            Assert.IsNotNull(users, UserWasNotPassedToStoreError);
            Assert.AreEqual(1, users.Length, UserWasNotPassedToStoreError);

            User createdUser = users[0];

            Assert.That.All(
                () => Assert.AreEqual("user name", createdUser.Name, WrongUserArgumentsPassedToDbError),
                () => Assert.AreEqual("user@email.com", createdUser.Email, WrongUserArgumentsPassedToDbError),
                () => Assert.AreEqual(42, createdUser.Age, WrongUserArgumentsPassedToDbError)
                );

        }

        #region errors

        private readonly string OpenSessionNotCalledError =
            @"
Error: OpenSession was not called in UsersRepository.CreateNewUser
------------------------------------------------------------------
    You must open a connection to the database before attempting save:
    Add the following code to UsersController.CreateNewUser:

    using (var session = _documentStoreFactory.Store.OpenSession())
    {
        session.Store(newUser);
    
        session.SaveChanges();
    }
";
        private readonly string StoreNotCalledError =
            @"
Error: Store was not called in UsersRepository.CreateNewUser
------------------------------------------------------------
    You must pass the new user to the database before attempting save:
    Add the following code to UsersController.CreateNewUser:

    using (var session = _documentStoreFactory.Store.OpenSession())
    {
        session.Store(newUser);
    
        session.SaveChanges();
    }
";

        private readonly string UserWasNotPassedToStoreError =
            @"
Error: Store was called without the user to create
--------------------------------------------------
    You must create a new user instance and pass it as an argument to session.Store:

    var user = new User();
        
    ...

    session.Store(newUser);
    
";
        private readonly string WrongUserArgumentsPassedToDbError =
            @"
Error: wrong user arguments were passed to session.Store
--------------------------------------------------------
    You must create a new user instance, initialize it with the arguments passed to the method
    and pass it as an argument to session.Store:

 public int CreateNewUser(string name, string email, int age)
 {
     var newUser = new User
     {
         Name = name, 
         Email = email, 
         Age = age
     };
 
     using (var session = _documentStoreFactory.Store.OpenSession())
     {
         session.Store(newUser);
 
         session.SaveChanges();
     }
 }
";

        private readonly string SaveChangesNotCalledError =
            @"
Error: SaveChanges was not called in UsersRepository.CreateNewUser
------------------------------------------------------------------
    You must call SaveChanges in order to commit changes to the the database:
    Add the following code to UsersController.CreateNewUser:

    using (var session = _documentStoreFactory.Store.OpenSession())
    {
        session.Store(newUser);
    
        session.SaveChanges();
    }
";


        #endregion
    }
}
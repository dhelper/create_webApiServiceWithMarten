using System.Collections.Generic;
using System.Data;
using System.Linq;
using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using Marten;
using Marten.Linq;
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
        public void Task3_CreateNewUserShouldCallStoreWithUserData()
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

        [TestMethod]
        public void Task4_GetUserByIdShouldCallQuerySession()
        {
            using var autoFake = new AutoFake();

            var fakeDocumentStoreFactory = autoFake.Resolve<IDocumentStoreFactory>();
            var fakeDocumentStore = A.Fake<IDocumentStore>();
            A.CallTo(() => fakeDocumentStoreFactory.Store).Returns(fakeDocumentStore);

            var usersRepository = autoFake.Resolve<UsersRepository>();

            usersRepository.GetUserById(1);


            var wasCalled = TestUtilities.WasCalled(() => fakeDocumentStore.QuerySession());

            Assert.IsTrue(wasCalled, QuerySessionNotCalledError);
        }

        [TestMethod]
        public void Task4_GetUserByIdShouldCallLoad()
        {
            using var autoFake = new AutoFake();

            var user = new User
            {
                Id = 1,
                Name = "user1",
                Age = 42,
                Email = "user1@email.com"
            };

            var fakeDocumentStoreFactory = autoFake.Resolve<IDocumentStoreFactory>();
            var fakeDocumentStore = A.Fake<IDocumentStore>();
            A.CallTo(() => fakeDocumentStoreFactory.Store).Returns(fakeDocumentStore);
            var fakeSession = A.Fake<IQuerySession>();
            A.CallTo(() => fakeDocumentStore.QuerySession()).Returns(fakeSession);
            A.CallTo(() => fakeSession.Load<User>(1)).Returns(user);

            var usersRepository = autoFake.Resolve<UsersRepository>();

            var userById = usersRepository.GetUserById(1);

            var fakeObjectCall = TestUtilities.GetMethodCall(() => fakeSession.Load<User>(0));

            Assert.IsNotNull(fakeObjectCall, QuerySessionLoadNotCalledError);
            Assert.AreEqual(1, fakeObjectCall.Arguments.Get<int>(0), IdNotPassedToQueryError);
            Assert.AreEqual(user, userById, UserNotReturnedFromGetUserByIdError);
        }

        [TestMethod]
        public void Task5_DeleteUserShouldCallOpenSession()
        {
            using var autoFake = new AutoFake();

            var fakeDocumentStoreFactory = autoFake.Resolve<IDocumentStoreFactory>();
            var fakeDocumentStore = A.Fake<IDocumentStore>();
            A.CallTo(() => fakeDocumentStoreFactory.Store).Returns(fakeDocumentStore);

            var usersRepository = autoFake.Resolve<UsersRepository>();

            usersRepository.DeleteUser(100);


            var wasCalled = TestUtilities.WasCalled(() => fakeDocumentStore.OpenSession(DocumentTracking.None, IsolationLevel.Unspecified));

            Assert.IsTrue(wasCalled, OpenSessionNotCalledError);
        }


        [TestMethod]
        public void Task5_CreateNewUserShouldCallDelete()
        {
            using var autoFake = new AutoFake();

            var fakeDocumentStoreFactory = autoFake.Resolve<IDocumentStoreFactory>();
            var fakeDocumentStore = A.Fake<IDocumentStore>();
            A.CallTo(() => fakeDocumentStoreFactory.Store).Returns(fakeDocumentStore);
            var fakeDocumentSession = A.Fake<IDocumentSession>();
            A.CallTo(() => fakeDocumentStore.OpenSession(A<DocumentTracking>._, A<IsolationLevel>._))
                .Returns(fakeDocumentSession);

            var usersRepository = autoFake.Resolve<UsersRepository>();

            usersRepository.DeleteUser(100);

            var fakeObjectCall = TestUtilities.GetMethodCall(() => fakeDocumentSession.Delete<User>(0));

            Assert.IsNotNull(fakeObjectCall, DeleteNotCalledError);
            Assert.AreEqual(100, fakeObjectCall.Arguments.Get<int>(0), IdNotPassedToDeleteError);
        }

        [TestMethod]
        public void Task5_CreateNewUserShouldCallSaveChanges()
        {
            using var autoFake = new AutoFake();

            var fakeDocumentStoreFactory = autoFake.Resolve<IDocumentStoreFactory>();
            var fakeDocumentStore = A.Fake<IDocumentStore>();
            A.CallTo(() => fakeDocumentStoreFactory.Store).Returns(fakeDocumentStore);
            var fakeDocumentSession = A.Fake<IDocumentSession>();
            A.CallTo(() => fakeDocumentStore.OpenSession(A<DocumentTracking>._, A<IsolationLevel>._))
                .Returns(fakeDocumentSession);

            var usersRepository = autoFake.Resolve<UsersRepository>();

            usersRepository.DeleteUser(100);

            var wasCalled = TestUtilities.WasCalled(() => fakeDocumentSession.SaveChanges());

            Assert.IsTrue(wasCalled, SaveChangesNotCalledError);
        }

        #region errors

        private readonly string OpenSessionNotCalledError =
            @"
Error: OpenSession was not called in UsersRepository
----------------------------------------------------
    You must open a connection to the database before attempting save:
    Add the following code to your method:

    using (var session = _documentStoreFactory.Store.OpenSession())
    {
        ...
    }
";
        private readonly string QuerySessionNotCalledError =
            @"
Error: QuerySession was not called 
----------------------------------
    You must open a connection to the database before attempting to read values:
    Add the following code to your method:

    using (var session = _documentStoreFactory.Store.QuerySession())
    {

    }
";

        private readonly string StoreNotCalledError =
            @"
Error: Store was not called in UsersRepository.CreateNewUser
------------------------------------------------------------
    You must pass the new user to the database before attempting save:
    Add the following code to UsersRepository.CreateNewUser:

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
    Add the following code to your method:

    using (var session = _documentStoreFactory.Store.OpenSession())
    {
       ...
    
        session.SaveChanges();
    }
";

        private readonly string QuerySessionLoadNotCalledError =
            @"
Error: QuerySession.Load was not called in UsersRepository.GetUserById
----------------------------------------------------------------------
    You must call Load in order to return the user with that id 
    Add the following code to UsersRepository.GetUserById:

    using (var session = _documentStoreFactory.Store.QuerySession())
    {
        return session.Query<User>(id);
    }
";

        private readonly string IdNotPassedToQueryError =
            @"
Error: QuerySession.Load did not receive the correct user id 
------------------------------------------------------------
    You must pass the user's id as a parameter to the Load method call in order to return the user with that id 
    Add the following code to UsersRepository.GetUserById:

    using (var session = _documentStoreFactory.Store.QuerySession())
    {
        return session.Load<User>(id);
    }
";

        private readonly string UserNotReturnedFromGetUserByIdError =
            @"
Error: UsersRepository.GetUserById did not return the correct user
------------------------------------------------------------------
    You must return the value loaded from the DB from method call
    Add the following code to UsersRepository.GetUserById:

    using (var session = _documentStoreFactory.Store.QuerySession())
    {
        return session.Load<User>(id);
    }
";

        private readonly string DeleteNotCalledError =
            @"
Error: Delete was not called in UsersRepository.Delete
------------------------------------------------------------------
    You must call SaveChanges in order to commit changes to the the database:
    Add the following code to your method:

    using (var session = _documentStoreFactory.Store.OpenSession())
    {
       ...
    
        session.Delete<User>(id);
    }
";

        private readonly string IdNotPassedToDeleteError =
            @"
Error: QuerySession.Delete did not receive the correct user id 
--------------------------------------------------------------
    You must pass the user's id as a parameter to the Delete method call 
    in order to remove that the user from the DB.

    Add the following code to UsersRepository.DeleteUser:

    using (var session = _documentStoreFactory.Store.OpenSession())
    {
        session.Delete<User>(id);

        session.SaveChanges();
    }
";

        #endregion
    }
}
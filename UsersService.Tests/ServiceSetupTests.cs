using System.Linq;
using System.Reflection;
using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using UsersService.Controllers;
using UsersService.DataAccess;

namespace UsersService.Tests
{
    [TestClass]
    public class ServiceSetupTests
    {
        [TestMethod]
        public void Task1_RepositoryDependenciesShouldBeRegistered()
        {
            var serviceCollection = new ServiceCollection();
            IConfiguration fakeConfiguration = A.Fake<IConfiguration>();
            serviceCollection.AddScoped(provider => fakeConfiguration);

            var startup = new Startup(fakeConfiguration);

            startup.ConfigureServices(serviceCollection);

            var result = serviceCollection.BuildServiceProvider().GetService<IUsersRepository>();

            Assert.IsNotNull(result, UserRepositoryNotRegisteredError);
            Assert.IsInstanceOfType(result, typeof(UsersRepository), UserRepositoryWrongClassRegisteredError);
        }

        [TestMethod]
        public void Task3_StoreFactoryDependencyShouldBeRegistered()
        {
            var serviceCollection = new ServiceCollection();
            IConfiguration fakeConfiguration = A.Fake<IConfiguration>();
            serviceCollection.AddScoped(provider => fakeConfiguration);

            var startup = new Startup(fakeConfiguration);

            startup.ConfigureServices(serviceCollection);

            var result = serviceCollection.BuildServiceProvider().GetService<IDocumentStoreFactory>();

            Assert.IsNotNull(result, DocumentStoreFactoryNotRegisteredError);
        }

        [TestMethod]
        public void Task1_UsersControllerShouldHaveOnlyOneConstructor()
        {
            var constructorInfos = typeof(UsersController).GetConstructors();

            Assert.AreEqual(1, constructorInfos.Length, UserControllerHasMoreThanOneConstructorError);
        }

        [TestMethod]
        public void Task1_UserRepositoryShouldBeInjectedToController()
        {
            var constructorInfos = typeof(UsersController).GetConstructors();

            var parameterFound = constructorInfos[0].GetParameters().Any(p => p.ParameterType.FullName == typeof(IUsersRepository).FullName);

            Assert.IsTrue(parameterFound, UserControllerConstructorDoesNotReceiveIUserRepository);
        }

        [TestMethod]
        public void Task1_UserRepositoryShouldBeStoredInUsersController()
        {
            using var fake = new AutoFake();

            var fakeUsersRepository = A.Fake<IUsersRepository>();
            fake.Provide(fakeUsersRepository);

            var usersController = fake.Resolve<UsersController>();

            var fieldInfos = usersController.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            var foundField = fieldInfos.FirstOrDefault(info => info.FieldType.FullName == typeof(IUsersRepository).FullName);

            Assert.IsNotNull(foundField, UsersControllersDoNotHaveFieldOfTypeUserRepositoryError);

            var result = foundField.GetValue(usersController);
            Assert.AreSame(fakeUsersRepository, result, FieldDoesNotHaveValuePassedToConstructorError);
        }

        [TestMethod]
        public void Task3_UsersRepositoryShouldHaveOnlyOneConstructor()
        {
            var constructorInfos = typeof(UsersRepository).GetConstructors();

            Assert.AreEqual(1, constructorInfos.Length, UsersRepositoryHasMoreThanOneConstructorError);
        }

        [TestMethod]
        public void Task3_DocumentStoreFactoryShouldBeInjectedToUsersRepository()
        {
            var constructorInfos = typeof(UsersRepository).GetConstructors();

            var parameterFound = constructorInfos[0].GetParameters().Any(p => p.ParameterType.FullName == typeof(IDocumentStoreFactory).FullName);

            Assert.IsTrue(parameterFound, UsersRepositoryConstructorDoesNotReceiveIDocumentStoreFactory);
        }

        [TestMethod]
        public void Task3_DocumentStoreFactoryShouldBeStoredInUsersRepository()
        {
            using var fake = new AutoFake();

            var fakeDocumentStoreFactory = A.Fake<IDocumentStoreFactory>();
            fake.Provide(fakeDocumentStoreFactory);

            var usersController = fake.Resolve<UsersRepository>();

            var fieldInfos = usersController.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            var foundField = fieldInfos.FirstOrDefault(info => info.FieldType.FullName == typeof(IDocumentStoreFactory).FullName);

            Assert.IsNotNull(foundField, UsersRepositoryDoNotHaveFieldOfTypeDocumentStoreFactoryError);

            var result = foundField.GetValue(usersController);
            Assert.AreSame(fakeDocumentStoreFactory, result, DocumentStoreFactoryFieldDoesNotHaveValuePassedToConstructorError);
        }

        #region errors
        private readonly string UserRepositoryNotRegisteredError =
            @"
Error: UserRepository dependency not registered
-----------------------------------------------
    Add the following line to ConfigureServices (Startup.cs):

    services.AddScoped<IUsersRepository, UsersRepository>();
";

        private readonly string UserRepositoryWrongClassRegisteredError =
            @"
Error: Wrong dependency registered as IUserRepository
-----------------------------------------------------
    Add the following line to ConfigureServices (Startup.cs):

    services.AddScoped<IUsersRepository, UsersRepository>();
";


        private readonly string UserControllerHasMoreThanOneConstructorError =
            @"
Error: UsersController should have only one constructor
-------------------------------------------------------
    Make sure that UsersController has only one constructor
    Delete any unused constructors
";

        private readonly string UsersRepositoryHasMoreThanOneConstructorError =
            @"
Error: UsersRepository should have only one constructor
-------------------------------------------------------
    Make sure that UsersRepository has only one constructor
    Delete any unused constructors
";

        private readonly string UserControllerConstructorDoesNotReceiveIUserRepository =
            @"
Error: UsersController constructor must have one parameter of type IUserRepository
----------------------------------------------------------------------------------
    Make sure that UsersController constructor has one parameter of type IUserRepository.
    Examples:
        public UsersController(IUsersRepository usersRepository)
    or
        public UsersController(IUsersRepository usersRepository, ILogger<UsersController> logger)
    
";

        private readonly string UsersRepositoryConstructorDoesNotReceiveIDocumentStoreFactory =
            @"
Error: UsersRepository constructor must have one parameter of type IDocumentStoreFactory
----------------------------------------------------------------------------------
    Make sure that UsersRepository constructor has one parameter of type IDocumentStoreFactory.
    Examples:
        public UsersRepository(IDocumentStoreFactory documentStoreFactory)
    
";

        private readonly string UsersControllersDoNotHaveFieldOfTypeUserRepositoryError =
            @"
Error: UsersController is missing a field of type IUserRepository
-----------------------------------------------------------------
    Make sure that UsersController has one field type IUserRepository.
    And save the value passed from the constructor in that field

        class UsersController
        {
            private IUsersRepository usersRepository;
            ...
";

        private readonly string UsersRepositoryDoNotHaveFieldOfTypeDocumentStoreFactoryError =
            @"
Error: UsersRepository is missing a field of type IDocumentStoreFactory
-----------------------------------------------------------------
    Make sure that UsersRepository has one field type IDocumentStoreFactory.
    And save the value passed from the constructor in that field

        class UsersRepository
        {
            private IDocumentStoreFactory documentStoreFactory;
            ...
";

        private readonly string FieldDoesNotHaveValuePassedToConstructorError =
            @"
Error: UsersController does not store IUserRepository value in private field
----------------------------------------------------------------------------
    Make sure that UsersController has one field type IUserRepository.
    And save the value passed from the constructor in that field

        class UsersController
        {
            private IUsersRepository _usersRepository;
            
            ...
            
            public UsersController(IUsersRepository usersRepository)
            {
                _usersRepository = usersRepository;
            
";
        private readonly string DocumentStoreFactoryFieldDoesNotHaveValuePassedToConstructorError =
            @"
Error: UserRepository does not store IDocumentStoreFactory value in private field
----------------------------------------------------------------------------
    Make sure that UserRepository has one field type IDocumentStoreFactory.
    And save the value passed from the constructor in that field

        class UsersRepository
        {
            private IDocumentStoreFactory _documentStoreFactory;
            
            ...
            
            public UsersController(IDocumentStoreFactory documentStoreFactory)
            {
                _documentStoreFactory = documentStoreFactory;
            
";

        private readonly string DocumentStoreFactoryNotRegisteredError =
            @"
Error: Class which implements IDocumentStoreFactory was not registered in Startup.cs
-----------------------------------------------------------------------------------
    You need to implement IDocumentStoreFactory and register the new class in Startup.cs

    Example:
    services.AddScoped<IDocumentStoreFactory, DocumentStoreFactory>();
        
            
";

        #endregion
    }
}

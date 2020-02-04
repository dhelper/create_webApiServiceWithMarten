using System.Linq;
using System.Reflection;
using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void Task1_UsersControllerShouldHaveONlyOneConstructor()
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
            using(var fake = new AutoFake())
            {
                var fakeUsersRepository = A.Fake<IUsersRepository>();
                fake.Provide(fakeUsersRepository);

                var usersController = fake.Resolve<UsersController>();

                var fieldInfos = usersController.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

                var foundField = fieldInfos.FirstOrDefault(info => info.FieldType.FullName == typeof(IUsersRepository).FullName);

                Assert.IsNotNull(foundField, UsersControllersDoNotHaveFieldOfTypeUserRepositoryError);

                var result = foundField.GetValue(usersController);
                Assert.AreSame(fakeUsersRepository, result, FieldDoesNotHaveValuePassedToConstructorError);
            }
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

        #endregion
    }

}

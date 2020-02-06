# Create an ASP.NET Core service with PostgreSQL and Marten

## prerequisites
This exercise is meant top be run on Windows using Visual Studio 2019 and .NET Core 3.1.

### Docker
In this exercise we will use Linux containers and so you'll need to [download and install Docker](https://docs.docker.com/docker-for-windows/install/) on your develpoment machine.

### Visual Studio 2019 (and edition)
[Download the latest installer](https://visualstudio.microsoft.com/downloads/) and run it, make sure to install ASP .NET  development especialy .NET Core support

### .NET Core 3.1
If you do not have the LTS version of .NET Core 3.1 installed [download the SDK microsoft](https://dotnet.microsoft.com/download/dotnet-core)

## Testing your environment
Once you've opened the solution in Visual Studio, check that the current debug run is set to _docker compose_: ![debug docker compose](./Images/debug_docker_compose.PNG)

Press __F5__, after a few minutes a broswer window should open with the swagger page of the users service that has four different operations. 
![swagger on browser](./Images/swagger_on_browser.PNG)
When we add more functionlity to the users serviuce you can run the solution the same way, test the functionality and debug your code.

_Now you can stop the debugging session and start adding functionality to the new service._

## Running the tests
You can either run the tests from the command line using:   
_dotnet test UsersService.sln_  
or from within Visual Studio using:  
_Ctrl + R, A_ to run all tests

## 1 - Add User's repository to dependency injection.
In __Startup.cs__ file add the following line inside __ConfigureServices__ method:
```
services.AddScoped<IUsersRepository, UsersRepository>();
```
So that the method would look something like this:
```
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();

    services.AddScoped<IUsersRepository, UsersRepository>();
```
Next open __UsersController.cs__ and add _IUserRepository_ as a parameter to the class constructor:
```
public UsersController(IUsersRepository userRepository ...
```
Then store that value in a field inside __UsersController__

## 2 - Add call to user repository from controller
Inside _UsersController.CreateNewUser_ add a call to user repository passing the data we've recieved from User create.

## 3 - implement create new user


## 4 add call to get all users from controller

## 5 - implement get all users

## 6 - implement get user by id

## 7 - implement delete user by Id

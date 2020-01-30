# Create an ASP.NET Core service with PostgreSQL and Marten

## prerequisites
This exercise is meant top be run on Windows using Visual Studio 2019 and .NET Core 3.1.

### Docker
In this exercise we will use Linux containers and so you'll need to [download and install Docker](https://docs.docker.com/docker-for-windows/install/)

### Visual Studio 2019
[Download the latest installer](https://visualstudio.microsoft.com/downloads/), make sure to install ASP .NET  development especialy .NET Core support

### .NET Core 3.1
If you do not have the LTS version of .NET Core 3.1 installed [download the SDK microsoft](https://dotnet.microsoft.com/download/dotnet-core)

## Testing your environment
Once you've opened the solution in Visual Studio, check that the current debug run is set to _docker compose_ then press __F5__, after a few minutes a broswer window should open with the swagger page of the users service that has four different operations. When we add more functionlity to the users serviuce you can run the solution the same way, test the functionality and debug your code.

## 1 - Add User's repository to dependency injection.

## 2 - Add referece to user's repository in controller

## 3 - Add call to user repository from controller

## 4 - implement create new user

## 5 add call to get all users from controller

## 6 - implement get all users

## 7 - implement get user by id

## 8 - implement delete user by Id

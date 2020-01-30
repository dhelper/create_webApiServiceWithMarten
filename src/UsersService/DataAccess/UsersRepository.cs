using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using UsersService.Models;

namespace UsersService.DataAccess
{
    public class UsersRepository : IUsersRepository
    {
        private readonly string _connectionString;

        public UsersRepository(IConfiguration config)
        {
            _connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        }

        public string CreateNewUser(User user)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<User> GetAllUsers()
        {
            throw new System.NotImplementedException();
        }

        public User GetUserById(string id)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteUser(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}

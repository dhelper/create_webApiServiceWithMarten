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

        public int CreateNewUser(string name, string email, int age)
        {
            throw new System.NotImplementedException("UsersRepository.CreateNewUser(string name, string email, int age) not implemented yet read tasks.md for details");
        }

        public User GetUserById(int id)
        {
            throw new System.NotImplementedException("UsersRepository.GetUserById(int id) not implemented yet read tasks.md for details");
        }

        public void DeleteUser(int id)
        {
            throw new System.NotImplementedException("UsersRepository.DeleteUser(int id) not implemented yet read tasks.md for details");
        }
    }
}

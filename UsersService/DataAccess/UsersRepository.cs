using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using UsersService.Models;

namespace UsersService.DataAccess
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IDocumentStoreFactory _documentStoreFactory;

        public UsersRepository(IDocumentStoreFactory documentStoreFactory)
        {
            _documentStoreFactory = documentStoreFactory;
        }

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

            return newUser.Id;
        }

        public IEnumerable<User> GetAllUsers()
        {
            throw new System.NotImplementedException();
        }

        public User GetUserById(int id)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteUser(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}

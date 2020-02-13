using System;
using System.Collections.Generic;
using System.Linq;
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

        public User GetUserById(int id)
        {
            using (var session = _documentStoreFactory.Store.QuerySession())
            {
                return session.Load<User>(id);
            }
        }

        public void DeleteUser(int id)
        {
            using (var session = _documentStoreFactory.Store.OpenSession())
            {
                session.Delete<User>(id);

                session.SaveChanges();
            }
        }
    }
}

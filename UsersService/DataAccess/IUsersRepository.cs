using System.Collections.Generic;
using UsersService.Models;

namespace UsersService.DataAccess
{
    public interface IUsersRepository
    {
        string CreateNewUser(User user);
        IEnumerable<User> GetAllUsers();
        User GetUserById(string id);
        void DeleteUser(string id);
    }
}
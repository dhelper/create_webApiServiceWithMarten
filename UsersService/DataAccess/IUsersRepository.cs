using System.Collections.Generic;
using UsersService.Models;

namespace UsersService.DataAccess
{
    public interface IUsersRepository
    {
        int CreateNewUser(string name, string email, int age);
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        void DeleteUser(int id);
    }
}
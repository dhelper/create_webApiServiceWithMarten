using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UsersService.DataAccess;
using UsersService.Models;

namespace UsersService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository userRepository, ILogger<UsersController> logger)
        {
            _usersRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="userCreateData">new user information</param>
        /// <returns>the created user</returns>
        [HttpPost]
        public int CreateNewUser([FromBody] UserCreate userCreateData)
        {
            var id = _usersRepository.CreateNewUser(userCreateData.Name, userCreateData.Email, userCreateData.Age);
            
            return id;
        }

        /// <summary>
        /// Get user by Id
        /// </summary>
        [HttpGet("{id}")]
        public User GetUserById(int id)
        {
            return _usersRepository.GetUserById(id);
        }

        /// <summary>
        /// Delete user by id
        /// </summary>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _usersRepository.DeleteUser(id);
        }
    }
}

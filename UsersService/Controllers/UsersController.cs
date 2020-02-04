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

        public UsersController(
            ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="userCreateData">new user information</param>
        /// <returns>the created user</returns>
        [HttpPost]
        public string CreateNewUser([FromBody] UserCreate userCreateData)
        {
            return null;
        }

        /// <summary>
        /// Returns all users 
        /// </summary>
        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            return Enumerable.Empty<User>();
        }

        /// <summary>
        /// Get user by Id
        /// </summary>
        [HttpGet("{id}")]
        public User Get(int id)
        {
            return null;
        }

        /// <summary>
        /// Delete user by id
        /// </summary>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}

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
        public int CreateNewUser([FromBody] UserCreate userCreateData)
        {
            throw new NotImplementedException("CreateNewUser is not implemented yet, check tasks.md for instructions");
        }

        /// <summary>
        /// Get user by Id
        /// </summary>
        [HttpGet("{id}")]
        public User GetUserById(int id)
        {
            throw new NotImplementedException("GetUserById is not implemented yet, check tasks.md for instructions");
        }

        /// <summary>
        /// Delete user by id
        /// </summary>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException("Delete is not implemented yet, check tasks.md for instructions");
        }
    }
}

namespace UsersService.Controllers
{
    /// <summary>
    /// Class for creating a new user
    /// </summary>
    public class UserCreate
    {
        /// <summary>
        /// The user's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The user's email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's age at the time of creation
        /// </summary>
        public int Age { get; set; }
    }
}
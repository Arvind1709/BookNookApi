namespace TheBookNookApi.Dtos
{
    /// <summary>
    /// Data Transfer Object for user login.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Username or email used for login.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password associated with the user.
        /// </summary>
        public string Password { get; set; }
    }

}

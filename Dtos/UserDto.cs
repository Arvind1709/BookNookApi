namespace TheBookNookApi.Dtos
{
    #region User DTO

    /// <summary>
    /// Data Transfer Object representing a user and their details.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Username of the user.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Role assigned to the user (default is "Customer").
        /// </summary>
        public string Role { get; set; } = "Customer";

        /// <summary>
        /// Full name of the user.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Contact number of the user.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;
    }

    #endregion
}

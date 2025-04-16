using TheBookNookApi.Model;

namespace TheBookNookApi.Services.Interfaces
{
    /// <summary>
    /// Interface for user-related operations.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retrieves all users.
        /// </summary>
        Task<IEnumerable<UserModel>> GetUsersAsync();

        /// <summary>
        /// Retrieves a specific user by ID.
        /// </summary>
        Task<UserModel?> GetUserByIdAsync(int id);
        ///<summary>
        /// Retrieves a specific user by username.
        ///</summary>
        Task<UserModel> GetUserByUsernameAsync(string username);

        /// <summary>
        /// Creates a new user.
        /// </summary>
        Task<UserModel?> CreateUserAsync(UserModel userModel);

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        Task<bool> UpdateUserAsync(int id, UserModel userModel);

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        Task<bool> DeleteUserAsync(int id);
    }
}

using TheBookNookApi.Model;

namespace TheBookNookApi.Services.Interfaces
{
    #region ITokenService Interface

    /// <summary>
    /// Provides method to generate JWT token for authenticated users.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user model containing identity information.</param>
        /// <returns>A JWT token string.</returns>
        string GenerateJwtToken(UserModel user);
    }

    #endregion
}

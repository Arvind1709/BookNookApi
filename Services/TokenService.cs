using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TheBookNookApi.Model;
using TheBookNookApi.Services.Interfaces;

namespace TheBookNookApi.Services
{
    //public class TokenService : ITokenService
    //{
    //    private readonly IConfiguration _configuration;

    //    public TokenService(IConfiguration configuration)
    //    {
    //        _configuration = configuration;
    //    }

    //    public string GenerateJwtToken(UserModel user)
    //    {
    //        var claims = new[]
    //        {
    //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    //        new Claim(ClaimTypes.Name, user.Username),
    //        new Claim(ClaimTypes.Role, user.Role.ToString())
    //    };

    //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
    //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //        var token = new JwtSecurityToken(
    //            issuer: _configuration["Jwt:Issuer"],
    //            audience: _configuration["Jwt:Audience"],
    //            claims: claims,
    //            expires: DateTime.Now.AddHours(1),
    //            signingCredentials: creds
    //        );

    //        return new JwtSecurityTokenHandler().WriteToken(token);
    //    }
    //}

    #region TokenService Implementation

    /// <summary>
    /// Service to handle generation of JWT tokens.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor to inject configuration settings.
        /// </summary>
        /// <param name="configuration">Application configuration to access JWT settings.</param>
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generates a secure JWT token based on the user's identity and role.
        /// </summary>
        /// <param name="user">The user for whom to generate the token.</param>
        /// <returns>JWT token as a string.</returns>
        public string GenerateJwtToken(UserModel user)
        {
            // Define user claims (identity + role)
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // User ID
            new Claim(ClaimTypes.Name, user.Username),                 // Username
            new Claim(ClaimTypes.Role, user.Role.ToString())           // User Role (e.g., Admin, Customer)
        };

            // Generate security key using secret key from configuration
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // Define signing credentials with chosen algorithm
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Token valid for 1 hour
                signingCredentials: creds
            );

            // Return serialized token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    #endregion
}

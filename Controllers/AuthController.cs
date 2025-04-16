//using Microsoft.AspNetCore.Mvc;
//using TheBookNookApi.Dtos;
//using TheBookNookApi.Services.Interfaces;

#region Using Directives

using Microsoft.AspNetCore.Mvc;
using TheBookNookApi.Dtos;
using TheBookNookApi.Services.Interfaces;

#endregion

namespace TheBookNookApi.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class AuthController : ControllerBase
    //{
    //    //private readonly IConfiguration _configuration;
    //    //private readonly BookNookDbContext _context;
    //    private readonly IUserService _userService;
    //    private readonly ITokenService _tokenService;


    //    public AuthController(IUserService userService, ITokenService tokenService)
    //    {
    //        //_configuration = configuration;IConfiguration configuration, BookNookDbContext context,
    //        //_context = context;
    //        _userService = userService;
    //        _tokenService = tokenService;
    //    }

    //    [HttpPost("login")]
    //    public async Task<IActionResult> Login(LoginDto loginDto)
    //    {
    //        // var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);
    //        var user = await _userService.GetUsersAsync()
    //            .ContinueWith(t => t.Result.FirstOrDefault(u => u.Username == loginDto.Username));
    //        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
    //        {
    //            return Unauthorized("Invalid username or password");
    //        }

    //        // var token = GenerateJwtToken(user);
    //        var token = _tokenService.GenerateJwtToken(user);
    //        return Ok(new
    //        {
    //            token,
    //            username = user.Username,
    //            role = user.Role.ToString()  // 👈 Add this line
    //        });
    //    }

    //    //private string GenerateJwtToken(UserModel user)
    //    //{
    //    //    var claims = new[]
    //    //    {
    //    //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    //    //    new Claim(ClaimTypes.Name, user.Username),
    //    //    // You can add roles/permissions here
    //    //};

    //    //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
    //    //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //    //    var token = new JwtSecurityToken(
    //    //        issuer: _configuration["Jwt:Issuer"],
    //    //        audience: _configuration["Jwt:Audience"],
    //    //        claims: claims,
    //    //        expires: DateTime.Now.AddHours(1),
    //    //        signingCredentials: creds
    //    //    );

    //    //    return new JwtSecurityTokenHandler().WriteToken(token);
    //    //}

    //    //private string GenerateJwtToken(UserModel user)
    //    //{
    //    //    var claims = new[]
    //    //    {
    //    //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    //    //        new Claim(ClaimTypes.Name, user.Username),
    //    //        //new Claim(ClaimTypes.Role, user.Role) // Example: "Admin", "User"
    //    //        //new Claim(ClaimTypes.Role, user.Role ?? "User") // Default to "User"
    //    //         new Claim(ClaimTypes.Role, user.Role.ToString()) // ✅ Convert enum to string
    //    //    };

    //    //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
    //    //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //    //    var token = new JwtSecurityToken(
    //    //        issuer: _configuration["Jwt:Issuer"],
    //    //        audience: _configuration["Jwt:Audience"],
    //    //        claims: claims,
    //    //        expires: DateTime.Now.AddHours(1),
    //    //        signingCredentials: creds
    //    //    );

    //    //    return new JwtSecurityTokenHandler().WriteToken(token);
    //    //}


    //}
    #region AuthController

    /// <summary>
    /// Handles authentication operations like user login.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Private Fields

        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to inject required services.
        /// </summary>
        /// <param name="userService">Service for retrieving users.</param>
        /// <param name="tokenService">Service for generating JWT tokens.</param>
        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        #endregion

        #region Login Endpoint

        /// <summary>
        /// Authenticates the user and returns a JWT token with role.
        /// </summary>
        /// <param name="loginDto">User login credentials.</param>
        /// <returns>JWT token and user details on success, Unauthorized on failure.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            // Retrieve user from database using username
            var user = await _userService.GetUserByUsernameAsync(loginDto.Username);
            // Check if user exists and password is correct
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid username or password");
            }

            // Generate JWT token using TokenService
            var token = _tokenService.GenerateJwtToken(user);

            // Return token and additional user info
            return Ok(new
            {
                token,
                username = user.Username,
                role = user.Role.ToString()
            });
        }

        #endregion
    }

    #endregion
}

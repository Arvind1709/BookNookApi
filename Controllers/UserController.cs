using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheBookNookApi.Model;
using TheBookNookApi.Services.Interfaces;


namespace TheBookNookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        #region Constructor
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion
        #region GET: Get all users
        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userService.GetUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching users.", details = ex.Message });
            }
        }
        #endregion
        #region GET: Get user by ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserModel(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                return user == null ? NotFound() : Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching user.", details = ex.Message });
            }
        }
        #endregion
        #region POST: Create a new user
        [HttpPost("create")]
        public async Task<IActionResult> PostUserModel(UserModel userModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdUser = await _userService.CreateUserAsync(userModel);
                return CreatedAtAction(nameof(GetUserModel), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating user.", details = ex.Message });
            }
        }
        #endregion
        #region UPDATE: Update an existing user
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserModel userModel)
        {
            try
            {
                var result = await _userService.UpdateUserAsync(id, userModel);
                return result ? NoContent() : NotFound();
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(new { message = argEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating user.", details = ex.Message });
            }
        }
        #endregion
        #region DELETE: Delete a user
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUserModel(int id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                return result ? NoContent() : NotFound(new { message = "User not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting user.", details = ex.Message });
            }
        }
        #endregion
    }
}

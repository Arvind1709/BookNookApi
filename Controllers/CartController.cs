using Microsoft.AspNetCore.Mvc;
using TheBookNookApi.Dtos;
using TheBookNookApi.Services.Interfaces;

namespace TheBookNookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        #region AddToCart
        /// <summary>
        /// Adds an item to the user's cart.
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto cartItem)
        {
            if (cartItem == null || cartItem.UserId <= 0 || cartItem.BookId <= 0 || cartItem.Quantity <= 0)
                return BadRequest("Invalid cart item data.");

            try
            {
                var result = await _cartService.AddToCartAsync(cartItem);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding book to cart.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        #endregion

        #region GetCartByUserId
        /// <summary>
        /// Retrieves the cart for a specific user including book details.
        /// </summary>
        [HttpGet("GetCartByUserId/{userId}")]
        public async Task<IActionResult> GetCartByUserId(int userId)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null)
                return NotFound("Cart not found for the user");

            return Ok(cart);
        }
        #endregion

        #region Stored Procedure Endpoints
        /// <summary>
        /// Executes stored procedure to get cart for a user.
        /// </summary>
        [HttpGet("sp/GetCartByUserId/{userId}")]
        public async Task<IActionResult> GetCartByUserIdSP(int userId)
        {
            var result = await _cartService.GetCartByUserIdSPAsync(userId);
            if (result == null || !result.Any())
                return NotFound("Cart not found for the user");

            return Ok(result);
        }
        #endregion
    }
}

using Microsoft.AspNetCore.Mvc;
using TheBookNookApi.Dtos;
using TheBookNookApi.Services.Interfaces;

namespace TheBookNookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #region Place Order (from Cart)

        /// <summary>
        /// Places an order based on items in the user's cart.
        /// </summary>
        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] CreateOrderDto orderDto)
        {
            return await _orderService.PlaceOrderAsync(orderDto);
        }

        #endregion

        #region Shop Now (Single Book Order)

        /// <summary>
        /// Places an immediate order for a single book without using the cart.
        /// </summary>
        [HttpPost("ShopNow")]
        public async Task<IActionResult> ShopNow([FromBody] ShopNowOrderDto orderDto)
        {
            return await _orderService.ShopNowAsync(orderDto);
        }

        #endregion
    }
}

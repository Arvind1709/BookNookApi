using Microsoft.AspNetCore.Mvc;
using TheBookNookApi.Dtos;

namespace TheBookNookApi.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IActionResult> PlaceOrderAsync(CreateOrderDto orderDto);
        Task<IActionResult> ShopNowAsync(ShopNowOrderDto orderDto);
    }
}

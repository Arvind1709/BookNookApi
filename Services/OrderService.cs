using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheBookNookApi.AppDbContext;
using TheBookNookApi.Dtos;
using TheBookNookApi.Model;
using TheBookNookApi.Services.Interfaces;

namespace TheBookNookApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly BookNookDbContext _context;

        public OrderService(BookNookDbContext context)
        {
            _context = context;
        }

        #region Place Order from Cart

        public async Task<IActionResult> PlaceOrderAsync(CreateOrderDto orderDto)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Book)
                .FirstOrDefaultAsync(c => c.UserId == orderDto.UserId);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
                return new BadRequestObjectResult("Cart is empty or not found!");

            foreach (var item in cart.CartItems)
            {
                if (item.Book.Stock < item.Quantity)
                    return new BadRequestObjectResult($"Not enough stock for book '{item.Book.Title}'. Available: {item.Book.Stock}");
            }

            decimal totalPrice = cart.CartItems.Sum(ci => ci.Quantity * ci.Book.Price);

            var order = new OrderModel
            {
                UserId = cart.UserId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalPrice,
                OrderItems = cart.CartItems.Select(ci => new OrderItemModel
                {
                    BookId = ci.BookId,
                    Quantity = ci.Quantity,
                    Price = ci.Book.Price
                }).ToList()
            };

            _context.Orders.Add(order);

            foreach (var item in cart.CartItems)
            {
                item.Book.Stock -= item.Quantity;
            }

            _context.CartItems.RemoveRange(cart.CartItems);
            _context.Carts.Remove(cart);

            await _context.SaveChangesAsync();

            return new OkObjectResult(new { Message = "Order placed successfully", OrderId = order.Id });
        }

        #endregion

        #region Shop Now

        public async Task<IActionResult> ShopNowAsync(ShopNowOrderDto orderDto)
        {
            if (orderDto == null || orderDto.UserId <= 0 || orderDto.BookId <= 0 || orderDto.Quantity <= 0)
                return new BadRequestObjectResult("Invalid order data.");

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == orderDto.BookId);

            if (book == null)
                return new NotFoundObjectResult("Book not found.");

            if (book.Stock < orderDto.Quantity)
                return new BadRequestObjectResult("Insufficient stock.");

            decimal totalPrice = book.Price * orderDto.Quantity;

            var newOrder = new OrderModel
            {
                UserId = orderDto.UserId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalPrice,
                OrderItems = new List<OrderItemModel>
            {
                new OrderItemModel
                {
                    BookId = orderDto.BookId,
                    Quantity = orderDto.Quantity,
                    Price = book.Price
                }
            }
            };

            book.Stock -= orderDto.Quantity;

            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new { message = "Order placed successfully", orderId = newOrder.Id });
        }

        #endregion
    }
}

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TheBookNookApi.AppDbContext;
using TheBookNookApi.Dtos;
using TheBookNookApi.Model;
using TheBookNookApi.Services.Interfaces;

namespace TheBookNookApi.Services
{
    public class CartService : ICartService
    {
        private readonly BookNookDbContext _context;
        private readonly ILogger<CartService> _logger;

        public CartService(BookNookDbContext context, ILogger<CartService> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region AddToCart
        /// <summary>
        /// Adds a book to the user's cart or updates quantity if it already exists.
        /// </summary>
        public async Task<string> AddToCartAsync(AddToCartDto cartItem)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == cartItem.UserId);
            if (cart == null)
            {
                cart = new CartModel
                {
                    UserId = cartItem.UserId,
                    CreatedDate = DateTime.UtcNow
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.BookId == cartItem.BookId);

            if (existingItem != null)
            {
                existingItem.Quantity += cartItem.Quantity;
                _context.CartItems.Update(existingItem);
            }
            else
            {
                var newCartItem = new CartItemModel
                {
                    CartId = cart.Id,
                    BookId = cartItem.BookId,
                    Quantity = cartItem.Quantity
                };
                _context.CartItems.Add(newCartItem);
            }

            await _context.SaveChangesAsync();
            return "Book added to cart successfully.";
        }
        #endregion

        #region GetCartByUserId
        /// <summary>
        /// Retrieves a user's cart including book details.
        /// </summary>
        public async Task<CartDto?> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Book)
                .Where(c => c.UserId == userId)
                .Select(c => new CartDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    CartItems = c.CartItems.Select(ci => new CartItemDto
                    {
                        Id = ci.Id,
                        BookId = ci.BookId,
                        Quantity = ci.Quantity,
                        BookTitle = ci.Book.Title,
                        Price = ci.Book.Price,
                        BookCover = ci.Book.BookCover
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }
        #endregion

        #region Stored Procedures
        public void AddToCartSP(int userId, int bookId, int quantity)
        {
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "AddToCart";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UserId", userId));
            cmd.Parameters.Add(new SqlParameter("@BookId", bookId));
            cmd.Parameters.Add(new SqlParameter("@Quantity", quantity));

            _context.Database.OpenConnection();
            cmd.ExecuteNonQuery();
            _context.Database.CloseConnection();
        }

        public async Task<List<CartItemModel>> GetCartByUserIdSPAsync(int userId)
        {
            return await _context.CartItems
                .FromSqlRaw("EXEC GetCartByUserId @UserId = {0}", userId)
                .ToListAsync();
        }
        #endregion
    }
}

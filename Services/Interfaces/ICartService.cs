using TheBookNookApi.Dtos;
using TheBookNookApi.Model;

namespace TheBookNookApi.Services.Interfaces
{
    /// <summary>
    /// Defines methods for managing cart operations.
    /// </summary>
    public interface ICartService
    {
        /// <summary>
        /// Adds a book to the user's cart or updates quantity if the item already exists.
        /// </summary>
        /// <param name="cartItem">The cart item to be added or updated.</param>
        /// <returns>A success message.</returns>
        Task<string> AddToCartAsync(AddToCartDto cartItem);

        /// <summary>
        /// Retrieves the cart for a specific user, including book details.
        /// </summary>
        /// <param name="userId">The ID of the user whose cart is being retrieved.</param>
        /// <returns>The user's cart details if found; otherwise, null.</returns>
        Task<CartDto?> GetCartByUserIdAsync(int userId);

        /// <summary>
        /// Adds a book to the user's cart using a stored procedure.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="bookId">The ID of the book to add.</param>
        /// <param name="quantity">The quantity of the book to add.</param>
        void AddToCartSP(int userId, int bookId, int quantity);

        /// <summary>
        /// Retrieves the cart items for a user using a stored procedure.
        /// </summary>
        /// <param name="userId">The ID of the user whose cart items are being retrieved.</param>
        /// <returns>A list of cart items for the user.</returns>
        Task<List<CartItemModel>> GetCartByUserIdSPAsync(int userId);
    }
}

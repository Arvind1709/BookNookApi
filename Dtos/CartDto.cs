namespace TheBookNookApi.Dtos
{
    /// <summary>
    /// Data Transfer Object representing a user's cart and its items.
    /// </summary>
    public class CartDto
    {
        /// <summary>
        /// Unique identifier for the cart.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID of the user who owns the cart.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// List of items contained in the cart.
        /// </summary>
        public List<CartItemDto> CartItems { get; set; }
    }

}

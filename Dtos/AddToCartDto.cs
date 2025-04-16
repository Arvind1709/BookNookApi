namespace TheBookNookApi.Dtos
{
    /// <summary>
    /// Data Transfer Object used to add a book to the user's shopping cart.
    /// </summary>
    public class AddToCartDto
    {
        /// <summary>
        /// The ID of the user adding the book to the cart.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The ID of the book being added to the cart.
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// The quantity of the book to add to the cart.
        /// </summary>
        public int Quantity { get; set; }
    }
}

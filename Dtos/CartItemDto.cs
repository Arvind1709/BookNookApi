namespace TheBookNookApi.Dtos
{
    #region DTOs

    /// <summary>
    /// Data Transfer Object representing a cart item along with book details.
    /// </summary>
    public class CartItemDto
    {
        /// <summary>
        /// Unique identifier for the cart item.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID of the book added to the cart.
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// Quantity of the book added to the cart.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Title of the book.
        /// </summary>
        public string BookTitle { get; set; }

        /// <summary>
        /// Price of the book.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Cover image of the book.
        /// </summary>
        public string BookCover { get; set; }
    }
    #endregion
}

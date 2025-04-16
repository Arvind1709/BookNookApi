using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TheBookNookApi.Model
{
    /// <summary>
    /// Represents an item inside a user's shopping cart.
    /// Each item is linked to a book.
    /// </summary>
    public class CartItemModel
    {
        /// <summary>
        /// Primary key for the cart item.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key referencing the cart to which this item belongs.
        /// </summary>
        [Required(ErrorMessage = "Cart ID is required.")]
        [Display(Name = "Cart")]
        public int CartId { get; set; }

        /// <summary>
        /// Navigation property for the parent cart.
        /// </summary>
        [ForeignKey("CartId")]
        [JsonIgnore]
        public virtual CartModel Cart { get; set; }

        /// <summary>
        /// Foreign key referencing the book being added to the cart.
        /// </summary>
        [Required(ErrorMessage = "Book ID is required.")]
        [Display(Name = "Book")]
        public int BookId { get; set; }

        /// <summary>
        /// Quantity of the book added to the cart.
        /// </summary>
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Navigation property for the book.
        /// </summary>
        public virtual BookModel Book { get; set; }
    }
}

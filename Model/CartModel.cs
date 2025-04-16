using System.ComponentModel.DataAnnotations;

namespace TheBookNookApi.Model
{
    /// <summary>
    /// Represents a shopping cart belonging to a user.
    /// Each cart can contain multiple cart items.
    /// </summary>
    public class CartModel
    {
        /// <summary>
        /// Primary key for the cart.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The ID of the user to whom the cart belongs.
        /// </summary>
        [Required(ErrorMessage = "User is required.")]
        [Display(Name = "User")]
        public int UserId { get; set; }

        /// <summary>
        /// The date when the cart was created.
        /// </summary>
        [Required(ErrorMessage = "Created Date is required.")]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Navigation property representing the collection of items in the cart.
        /// </summary>
        public virtual ICollection<CartItemModel> CartItems { get; set; } = new List<CartItemModel>();
    }
}

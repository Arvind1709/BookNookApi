using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBookNookApi.Model
{
    public class OrderItemModel
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the order this item belongs to.
        /// </summary>
        [Required]
        public int OrderId { get; set; }

        /// <summary>
        /// Navigation property to the order.
        /// </summary>
        [ForeignKey("OrderId")]
        public virtual OrderModel? Order { get; set; }

        /// <summary>
        /// Foreign key to the book being ordered.
        /// </summary>
        [Required]
        public int BookId { get; set; }

        /// <summary>
        /// Navigation property to the book.
        /// </summary>
        public virtual BookModel? Book { get; set; }

        /// <summary>
        /// Quantity of the book ordered.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        /// <summary>
        /// Price per unit at the time of order (snapshot of the price).
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // public BookModel Book { get; set; }
    }
}

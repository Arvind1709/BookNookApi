using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBookNookApi.Model
{
    public class OrderModel
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key referencing the user who placed the order.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Date and time when the order was placed.
        /// </summary>
        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Total amount of the order.
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Current status of the order. Possible values: Pending, Completed, Cancelled.
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        /// <summary>
        /// Navigation property - One order can contain multiple order items.
        /// </summary>
        public virtual ICollection<OrderItemModel>? OrderItems { get; set; } = new List<OrderItemModel>();
    }
}

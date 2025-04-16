namespace TheBookNookApi.Dtos
{
    /// <summary>
    /// Data Transfer Object for creating an order.
    /// </summary>
    public class CreateOrderDto
    {
        /// <summary>
        /// ID of the user placing the order.
        /// </summary>
        public int UserId { get; set; }
    }
}

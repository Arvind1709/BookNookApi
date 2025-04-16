using Microsoft.EntityFrameworkCore;
using TheBookNookApi.Model;

namespace TheBookNookApi.AppDbContext
{
    public class BookNookDbContext : DbContext
    {
        public BookNookDbContext(DbContextOptions<BookNookDbContext> options) : base(options)
        {
        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<BookModel> Books { get; set; }
        public DbSet<CartModel> Carts { get; set; }
        public DbSet<CartItemModel> CartItems { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderItemModel> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Add any additional configuration here
        }
    }
}

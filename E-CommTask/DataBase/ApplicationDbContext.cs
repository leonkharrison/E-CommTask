using E_CommTask.Data.Orders;
using Microsoft.EntityFrameworkCore;

namespace E_CommTask.DataBase
{
    public class ApplicationDbContext : DbContext
    {
        IConfiguration _configuration;
        string connectionString;
        public ApplicationDbContext( IConfiguration configuration ) : base()
        {
            _configuration = configuration;
            connectionString = _configuration[ "ConnectionString" ];
        }

        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        {
            optionsBuilder.UseSqlite( connectionString );
        }

        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            base.OnModelCreating( modelBuilder );

            modelBuilder.Entity<ProductOrders>().HasKey( op => new { op.Id } );

            modelBuilder.Entity<ProductOrders>().HasOne<Order>( po => po.Order ).WithMany( o => o.Products ).HasForeignKey( p => p.OrderId );
            modelBuilder.Entity<ProductOrders>().HasOne<Product>( po => po.Product ).WithMany( o => o.Orders ).HasForeignKey( p => p.ProductId );

            modelBuilder.Entity<Product>().HasData( productSeeds );
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOrders> ProductOrders { get; set; }


        List<Product> productSeeds = new List<Product>()
        {
            new Product() { Id = 1, Name = "Apple", Description = "Ripe and ready!", Price = 1.20m },
            new Product() { Id = 2, Name = "Cheddar Cheese", Description = "Extra mature!", Price = 3.50m },
            new Product() { Id = 3, Name = "Milk", Description = "Semi-Skimmed!", Price = 0.90m },
            new Product() { Id = 4, Name = "T-Shirt", Description = "Comfy!", Price = 19.99m }
        };
    }
}

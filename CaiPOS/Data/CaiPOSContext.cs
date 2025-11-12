using CaiPOS.Models;
using Microsoft.EntityFrameworkCore;

namespace CaiPOS.Data
{
    public class CaiPOSContext : DbContext
    {
        public CaiPOSContext(DbContextOptions<CaiPOSContext> options)
            : base(options)
        { 
        }

        public DbSet<CaiPOS.Models.UserManagement> UserManagement { get; set; }
        public DbSet<CaiPOS.Models.Product> Product { get; set; }
        public DbSet<CaiPOS.Models.ShoppingCar> Cart { get; set; }
        public DbSet<CaiPOS.Models.ShoppingCarItem> CartItem { get; set; }
        public DbSet<CaiPOS.Models.Order> Order { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserManagement>().HasKey(u => u.UserId);
        }
    }
}

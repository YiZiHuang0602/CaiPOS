using CaiPOS.Models;
using Microsoft.EntityFrameworkCore;

namespace CaiPOS.Data
{
    public class CaiPOSContext : DbContext
    {
        public CaiPOSContext(DbContextOptions<CaiPOSContext> options) : base(options) { }

        public DbSet<CaiPOS.Models.UserManagement> Users { get; set; }
        public DbSet<CaiPOS.Models.Product> Products { get; set; }
        public DbSet<CaiPOS.Models.ShoppingCar> ShoppingCar { get; set; }
        public DbSet<CaiPOS.Models.ShoppingCarItem> ShoppingCarItems { get; set; }
        public DbSet<CaiPOS.Models.Order> Orders { get; set; }

    }
}

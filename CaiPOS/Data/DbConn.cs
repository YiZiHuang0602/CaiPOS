using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CaiPOS.Model;

namespace CaiPOS.Data
{
    public class DbConn : DbContext
    {
        public DbConn(DbContextOptions<DbConn> options) : base(options) { }
        public DbSet<Products> Prroducts { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<ShoppingCar> ShoppingCar { get; set; }
        public DbSet<CarItem> CarItems { get; set; }
    }
}

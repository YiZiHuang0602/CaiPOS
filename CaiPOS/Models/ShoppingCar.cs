using System.ComponentModel.DataAnnotations;

namespace CaiPOS.Models
{
    public class ShoppingCar
    {
        [Key]
        public Guid CarId { get; set; } = new Guid();

        public Guid UserID { get; set; } = new Guid();

        public string ProductName { get; set; }

        public DateTime CreatedAt { get; set; }

        public int ProductCount { get; set; }

        public int TotalPrice { get; set; }
    }
}

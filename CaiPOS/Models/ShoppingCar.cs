using System.ComponentModel.DataAnnotations;

namespace CaiPOS.Models
{
    public class ShoppingCar
    {
        [Key]
        public Guid CarId { get; set; } = new Guid();

        public Guid UserID { get; set; } = new Guid();

        public DateTime CreatedAt { get; set; }

        public int TotalQuantity { get; set; }

        public int TotalAmount { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
namespace CaiPOS.Model
{
    public class ShoppingCar
    {
        [Key]
        public Guid ShoppingCarId { get; set; }
        public Guid UserId { get; set; } 
        public string ProductName { get; set; }
        public int ProductCount { get; set; }
        public int TotalPrice { get; set; }
    }
}

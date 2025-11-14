using System.ComponentModel.DataAnnotations;

namespace CaiPOS.Model
{
    public class CarItem
    {
        [Key]
        public Guid CarItemID { get; set; }
        public Guid ShoppingCarId { get; set; }
        public Guid ProductId { get; set; }
        public int quentity { get; set; }
        public int price { get; set; }
        public string? note { get; set; }
    }
}

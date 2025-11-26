using System.ComponentModel.DataAnnotations;

namespace CaiPOS.Models
{
    public class ShoppingCarItem
    {
        [Key]
        public Guid CarItemId { get; set; } = Guid.NewGuid();

        public Guid CarId { get; set; }

        public Guid ProductId { get; set; }

        [Required]
        public required string ProductName { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public string? Note { get; set; }
    }
}

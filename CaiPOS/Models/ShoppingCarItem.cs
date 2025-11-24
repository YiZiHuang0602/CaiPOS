using System.ComponentModel.DataAnnotations;

namespace CaiPOS.Models
{
    public class ShoppingCarItem
    {
        [Key]
        public Guid CarItemId { get; set; }

        public Guid CarId { get; set; }

        public Guid ProductId { get; set; }

        [Required]
        public required string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string? Note { get; set; }
    }
}

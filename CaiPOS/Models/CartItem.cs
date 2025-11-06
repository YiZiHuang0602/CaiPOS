using System.ComponentModel.DataAnnotations;

namespace CaiPOS.Models
{
    public class CartItem
    {
        public Guid CartItemId { get; set; }

        public Guid CartId { get; set; }

        public Guid ProductId { get; set; }

        [Required]
        public required string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string? Note { get; set; }
    }
}

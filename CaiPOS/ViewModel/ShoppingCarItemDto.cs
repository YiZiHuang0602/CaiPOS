using System.ComponentModel.DataAnnotations;

namespace CaiPOS.ViewModel
{
    public class ShoppingCarItemDto
    {
        [Required]
        public required string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string? Note { get; set; }
    }
}

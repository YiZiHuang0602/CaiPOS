using System.ComponentModel.DataAnnotations;

namespace CaiPOS.ViewModel
{
    public class ProductDto
    {
        [Required]
        public required string ProductName { get; set; }

        public string? Category { get; set; }

        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string? Status { get; set; }
    }
}

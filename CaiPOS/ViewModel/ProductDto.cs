using System.ComponentModel.DataAnnotations;

namespace CaiPOS.ViewModel
{
    public class ProductDto
    {
        [Required]
        public string ProductName { get; set; }

        [Required]
        public string? Category { get; set; }

        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string? Status { get; set; }
    }
}

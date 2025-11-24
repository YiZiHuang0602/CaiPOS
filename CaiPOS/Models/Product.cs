using System.ComponentModel.DataAnnotations;

namespace CaiPOS.Models
{
    public class Product
    {
        public Guid ProductId { get; set; } = Guid.NewGuid();
        public string? ProductNumber { get; set; }

        [Required]
        public required string ProductName { get; set; }

        [Required]
        public required string Category { get; set; }

        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public required string Status { get; set; }
    }
}

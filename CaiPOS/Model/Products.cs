using System.ComponentModel.DataAnnotations;
namespace CaiPOS.Model
{
    public class Products
    {
        [Key]
        public Guid ProductId { get; set; }
        public string ProductNumber { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public string? description { get; set; }
        public int Price { get; set; }

    }
}

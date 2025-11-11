using System.ComponentModel.DataAnnotations;

namespace CaiPOS.ViewModel
{
    public class ProductsDto
    {
        public string Category { get; set; }
        public string ProductName { get; set; }
        public string? description { get; set; }
        public int Price { get; set; }

    }
}

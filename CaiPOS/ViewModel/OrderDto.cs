using System.ComponentModel.DataAnnotations;

namespace CaiPOS.ViewModel
{
    public class OrderDto
    {
        public DateTime OrderDate { get; set; }

        public int TotalAmount { get; set; }

        [Required]
        public required string OrderStatus { get; set; }
    }
}

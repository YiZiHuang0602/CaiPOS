using System.ComponentModel.DataAnnotations;

namespace CaiPOS.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }

        public int MemberID { get; set; }

        public DateTime OrderDate { get; set; }

        public int TotalAmount { get; set; }

        [Required]
        public required string OrderStatus { get; set; }

    }
}

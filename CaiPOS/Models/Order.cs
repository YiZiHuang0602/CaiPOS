using System.ComponentModel.DataAnnotations;

namespace CaiPOS.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }

        public Guid UserId { get; set; }

        public Guid CarId { get; set; }

        public DateTime OrderDate { get; set; }

        public int TotalCount { get; set; }

        public int TotalPrice { get; set; }
    }
}

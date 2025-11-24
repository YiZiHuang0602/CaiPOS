using System.ComponentModel.DataAnnotations;

namespace CaiPOS.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }

        public int MemberID { get; set; }

        public DateTime OrderDate { get; set; }

        public int TotalCount { get; set; }

        public int TotalPrice { get; set; }
    }
}

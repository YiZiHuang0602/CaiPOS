using System.ComponentModel.DataAnnotations;

namespace CaiPOS.Models
{
    public class ShoppingCar
    {
        [Key]
        public Guid CartId { get; set; }

        public int MemberID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; }

        public int TotalQuantity { get; set; }

        public int TotalAmount { get; set; }
    }
}

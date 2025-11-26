using System.ComponentModel.DataAnnotations;

namespace CaiPOS.Models
{
    public class ShoppingCar
    {
        [Key]
        public Guid CarId { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; }

        public int ProductCount { get; set; }

        public int TotalPrice { get; set; }
    }
}

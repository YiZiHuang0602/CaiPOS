using System.ComponentModel.DataAnnotations;

namespace CaiPOS.ViewModel
{
    public class ShoppingCarDto
    {
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; }

        public int TotalQuantity { get; set; }

        public int TotalAmount { get; set; }
    }
}

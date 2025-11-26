using System.ComponentModel.DataAnnotations;

namespace CaiPOS.ViewModel
{
    public class ShoppingCarDto
    {
        public Guid CarId { get; set; }

        public int TotalAmount { get; set; }

        public int TotalPrice { get; set; }
    }
}

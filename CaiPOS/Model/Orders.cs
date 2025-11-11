using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace CaiPOS.Model
{
    public class Orders
    {
        [Key]
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public Guid ShoppingCarId { get; set; }
        public int TotalPrice { get; set; }
    }
}

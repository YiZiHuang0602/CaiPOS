using System.ComponentModel.DataAnnotations;

namespace CaiPOS.ViewModel
{
    public class ShoppingCarItemDto
    {
        public Guid CarItemId { get; set; }

        [Required(ErrorMessage = "請輸入商品名稱")]
        public required string ProductName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "數量必須大於 0")]
        public int Quantity { get; set; }

        public int Price { get; set; }

        public string? Note { get; set; }
    }
}

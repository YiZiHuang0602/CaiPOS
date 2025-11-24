using System.ComponentModel.DataAnnotations;

namespace CaiPOS.ViewModel
{
    public class ProductDto
    {
        [Required(ErrorMessage = "商品名稱為必填欄位")]
        public required string ProductName { get; set; }

        [Required(ErrorMessage = "種類為必填欄位")]
        [RegularExpression(@"^(主食\(飯類\)|主食\(麵類\)|湯品|炸物)$", ErrorMessage = "銷售狀態只能填「主食(飯類)」或「主食(麵類)」或「湯品」或「炸物」")]
        public required string Category { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "價格為必填欄位")]
        [Range(1, 500, ErrorMessage = "價格必須介於 1 到 500 元")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "銷售狀態為必填欄位")]
        [RegularExpression(@"^(熱賣中|售完)$", ErrorMessage = "銷售狀態只能填「熱賣中」或「售完」")]
        public string? Status { get; set; }
    }
}

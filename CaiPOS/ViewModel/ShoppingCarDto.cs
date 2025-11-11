namespace CaiPOS.ViewModel
{
    public class ShoppingCarDto
    {
        public List<ShoppingCarItemDto> ShoppingCarItems { get; set; }
        public int TotalAmount { get; set; }
        public int TotalPrice { get; set; }
    }
}

using CaiPOS.Data;
using CaiPOS.Models;
using CaiPOS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaiPOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCarController : ControllerBase
    {
        private readonly CaiPOSContext _context;

        public ShoppingCarController(CaiPOSContext context)
        {
            _context = context;
        }

        private Guid GetUserIdByName(string userName)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == userName).UserId;
        }

        private string GetProductNameById(Guid productId)
        {
            return _context.Products.FirstOrDefault(p => p.ProductId == productId).ProductName;
        }

        /*[HttpGet("GetShoppingCartData")]
        public async Task<List<ShoppingCarItemDto>> GetShoppingCarData(string userName)
        {
            List<ShoppingCarItemDto> items = new List<ShoppingCarItemDto>();
            var uId = GetUserIdByName(userName);
            var car = await _context.ShoppingCar.FirstOrDefaultAsync(c => c.UserId == uId);
            var item = _context.ShoppingCarItems.Where(ci => ci.CarId == car.carId).ToList();
            foreach (var i in item)
            {
                var dto = new ShoppingCarItemDto
                {
                    ProductName = GetProductNameById(i.ProductId),
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Note = i.Note
                };
                items.Add(dto);
            }
        }*/
    }
}

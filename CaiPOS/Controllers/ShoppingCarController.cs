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
        public async Task<List<ShoppingCarDto>> GetShoppingCarData()
        {
            var shoppingCarDatas = new List<ShoppingCarDto>();
            var userId = GetUserIdByName(userName);
            var shoppingCars = await _context.ShoppingCar.ToListAsync();
            foreach(var shoppingCar in shoppingCars)
            {
                var dto = new ShoppingCarDto
                {
                    CreatedAt = shoppingCar.CreatedAt,
                    TotalQuantity = shoppingCar.TotalQuantity,
                    TotalAmount = shoppingCar.TotalAmount
                };
                shoppingCarDatas.Add(dto);
            }
        }*/
    }
}

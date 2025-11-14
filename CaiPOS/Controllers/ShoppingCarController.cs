using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CaiPOS.Data;
using CaiPOS.Model;
using CaiPOS.ViewModel;

namespace Tsai.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShoppingCarController : Controller
    {
        private readonly DbConn _conn;
        public ShoppingCarController(DbConn conn)
        {
            _conn = conn;
        }

        private Guid GetUserIdByUserName(string userName)
        {
            return _conn.Users.FirstOrDefault(u => u.Name == userName).UserId;
        }

        private string GetProductNameById(Guid id)
        {
            return _conn.Prroducts.FirstOrDefault(p => p.ProductId == id).ProductName;
        }

        [HttpGet("GetShoppingCarItems")]
        public async Task<ApiResponse<List<ShoppingCarItemDto>>> GetShoppingCarItems(string userName)
        {
            try
            {
                List<ShoppingCarItemDto> items = new List<ShoppingCarItemDto>();
                var uid = GetUserIdByUserName(userName);
                var car = await _conn.ShoppingCar.FirstOrDefaultAsync(c => c.UserId == uid);
                var item = _conn.CarItems.Where(ci => ci.ShoppingCarId == car.ShoppingCarId).ToList();
                foreach (var i in item)
                {
                    var iDto = new ShoppingCarItemDto
                    {
                        ProductName = GetProductNameById(i.ProductId),
                        amount = i.quentity,
                        price = i.price,
                        note = i.note
                    };
                    items.Add(iDto);
                }

                return new ApiResponse<List<ShoppingCarItemDto>>
                {
                    Success = true,
                    Message = items.Count == 0 ? "幹什麼東西 怎麼她媽還是空的 !":"",
                    Data = items
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ShoppingCarItemDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }

        }

        [HttpGet("GetShoppingCar")]
        public async Task<ShoppingCarDto> GetShoppingCar(string userName)
        {
            var carItemsResponse = await GetShoppingCarItems(userName);
            var carItems = carItemsResponse.Data;
            return new ShoppingCarDto
            {
                ShoppingCarItems = carItems,
                TotalAmount = carItems.Count,
                TotalPrice = carItems.Sum(p => p.price)
            };
        }

        [HttpPost("CreateShoppingCar")]
        public async Task<ApiResponse> CreateShoppingCar(string userName)
        {
            var uid = GetUserIdByUserName(userName);
            var car = new ShoppingCar
            {
                ShoppingCarId = Guid.NewGuid(),
                UserId = uid
            };
            await _conn.ShoppingCar.AddAsync(car);
            await _conn.SaveChangesAsync();
            return new ApiResponse
            {
                Success = true,
                Message = "不把你錢包榨乾 我誓不罷休"
            };
        }
    }
}

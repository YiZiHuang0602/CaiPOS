using CaiPOS.Data;
using CaiPOS.Migrations;
using CaiPOS.Models;
using CaiPOS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            return user?.UserId ?? Guid.Empty;
        }

        private Guid GetUserIdFromToken()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                throw new Exception("Token 無效，沒有 Identity");

            // 從 Token 裡取 UserId
            var userIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "UserId");

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
                throw new Exception("Token 中沒有 UserId");

            return Guid.Parse(userIdClaim.Value);
        }


        private string GetProductNameById(Guid productId)
        {
            return _context.Products.FirstOrDefault(p => p.ProductId == productId).ProductName;
        }

        private int CalculateChickenWingPrice(int quantity)
        {
            int setCount = quantity / 3;
            int singleCount = quantity % 3;

            return (setCount * 50) + (singleCount * 20);
        }

        [HttpGet("GetShoppingCartData")]
        public async Task<ApiResponse<List<ShoppingCarItemDto>>> GetShoppingCarData(string userName)
        {
            List<ShoppingCarItemDto> items = new List<ShoppingCarItemDto>();
            var uId = GetUserIdByName(userName);
            if (GetUserIdByName(userName) == Guid.Empty)
            {
                return new ApiResponse<List<ShoppingCarItemDto>>
                {
                    Success = false,
                    Message = $"「{userName}」使用者不存在",
                    Data = null
                };
            }

            var car = await _context.ShoppingCar.FirstOrDefaultAsync(c => c.UserId == uId);
            if (car == null) return new ApiResponse<List<ShoppingCarItemDto>>
            {
                Success = false,
                Message = "購物車細項不存在，你可能還沒點餐喔",
                Data = null
            };

            var item = _context.ShoppingCarItems.Where(ci => ci.CarId == car.CarId).ToList();
            foreach (var i in item)
            {
                var dto = new ShoppingCarItemDto
                {
                    CarItemId = i.CarItemId,
                    ProductName = GetProductNameById(i.ProductId),
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Note = i.Note
                };
                items.Add(dto);
            }
            return new ApiResponse<List<ShoppingCarItemDto>>
            {
                Success = true,
                Message = "取得購物車細項資料成功",
                Data = items
            };
        }

        [HttpGet("GetShoppingCar")]
        public async Task<ApiResponse<ShoppingCarDto>> GetShoppingCar(string userName)
        {
            List<ShoppingCarDto> carDtos = new List<ShoppingCarDto>();

            var uId = GetUserIdByName(userName);
            if (GetUserIdByName(userName) == Guid.Empty)
            {
                return new ApiResponse<ShoppingCarDto>
                {
                    Success = false,
                    Message = $"「{userName}」使用者不存在",
                    Data = null
                };
            }

            var car = await _context.ShoppingCar.FirstOrDefaultAsync(c => c.UserId == uId);
            if (car == null) return new ApiResponse<ShoppingCarDto>
            {
                Success = false,
                Message = "購物車不存在，你可能還沒點餐喔",
                Data = null
            };

            return new ApiResponse<ShoppingCarDto>
            {
                Success = true,
                Message = "取得購物車資料成功",
                Data = new ShoppingCarDto
                {
                    CarId = car.CarId,
                    TotalAmount = car.ProductCount,
                    TotalPrice = car.TotalPrice
                }
            };
        }

        [HttpPost("AddToShoppingCar")]
        public async Task<ApiResponse> AddToShoppingCar(string userName, ShoppingCarItemDto req)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return new ApiResponse
                    {
                        Success = false,
                        Message = string.Join("; ", errors)
                    };
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
                if (user == null)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = $"使用者 {userName} 不存在"
                    };
                }

                var uId = user.UserId;

                var product = await _context.Products.FindAsync(req.ProductId);

                if (product == null)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = $"商品 (Id={req.ProductId}) 不存在"
                    };
                }

                var car = await _context.ShoppingCar.FirstOrDefaultAsync(c => c.UserId == uId);
                if (car == null)
                {
                    car = new ShoppingCar
                    {
                        UserId = uId,
                        CreatedAt = DateTime.Now,
                        ProductCount = 0,
                        TotalPrice = 0
                    };
                    _context.ShoppingCar.Add(car);
                    await _context.SaveChangesAsync();
                }

                int finalPrice = 0;
                if (req.ProductName == "香酥雞翅")
                {
                    finalPrice = CalculateChickenWingPrice(req.Quantity);
                }
                else
                {
                    finalPrice = product.Price * req.Quantity;
                }

                var carItem = new ShoppingCarItem
                {
                    CarItemId = Guid.NewGuid(),
                    CarId = car.CarId,
                    ProductId = product.ProductId,
                    ProductName = req.ProductName,
                    Quantity = req.Quantity,
                    Price = finalPrice,
                    Note = req.Note == null ? "" : req.Note
                };
                _context.ShoppingCarItems.Add(carItem);

                car.ProductCount += req.Quantity;
                car.TotalPrice += finalPrice;

                await _context.SaveChangesAsync();
                return new ApiResponse
                {
                    Success = true,
                    Message = "產品已成功加入購物車"
                };
            }
            catch (DbUpdateException dbEx)
            {
                // 更清楚地回傳 DB 錯誤（不要只顯示 InnerException）
                return new ApiResponse
                {
                    Success = false,
                    Message = $"加入購物車時發生資料庫錯誤: {dbEx.Message} {dbEx.InnerException?.Message}"
                };
            }
            catch (Exception ex)
            {
                // 回傳完整的例外訊息（開發或 debug 時候用；上線可改為更簡潔）
                return new ApiResponse
                {
                    Success = false,
                    Message = $"加入購物車時發生錯誤: {ex.Message}\n{ex.StackTrace}"
                };
            }
        }

        [HttpPatch("EditToShoppingCar")]
        public async Task<ApiResponse> EditToShoppingCar(Guid carItemId, ShoppingCarItemDto req)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return new ApiResponse
                    {
                        Success = false,
                        Message = string.Join("; ", errors)
                    };
                }
                var carItem = await _context.ShoppingCarItems.FirstOrDefaultAsync(ci => ci.CarItemId == carItemId);
                if (carItem == null)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "購物車細項不存在"
                    };
                }

                Product product = null;
                if (!string.IsNullOrEmpty(req.ProductName))
                {
                    product = await _context.Products.FirstOrDefaultAsync(p => p.ProductName == req.ProductName);

                    if (product == null)
                    {
                        return new ApiResponse
                        {
                            Success = false,
                            Message = $"找不到商品 {req.ProductName}"
                        };
                    }
                }

                int finalPrice = 0;
                if (req.ProductName == "香酥雞翅")
                {
                    finalPrice = CalculateChickenWingPrice(req.Quantity);
                }
                else
                {
                    finalPrice = product.Price * req.Quantity;
                }

                int oldQuantity = carItem.Quantity;
                int oldPrice = carItem.Price;

                // 更新購物車細項
                carItem.CarItemId = carItemId;
                carItem.ProductId = product?.ProductId ?? carItem.ProductId;
                carItem.ProductName = req.ProductName ?? carItem.ProductName;
                carItem.Quantity = req.Quantity;
                carItem.Price = finalPrice;
                carItem.Note = req.Note == null ? "": req.Note;

                var car = await _context.ShoppingCar.FirstOrDefaultAsync(c => c.CarId == carItem.CarId);
                if (car != null)
                {
                    await _context.SaveChangesAsync(); // 先儲存細項變更，讓 SumAsync 取到最新值

                    var totalQuantity = await _context.ShoppingCarItems
                        .Where(ci => ci.CarId == car.CarId)
                        .SumAsync(ci => ci.Quantity);

                    var totalPrice = await _context.ShoppingCarItems
                        .Where(ci => ci.CarId == car.CarId)
                        .SumAsync(ci => ci.Price);

                    car.ProductCount = totalQuantity;
                    car.TotalPrice = totalPrice;
                }
                else
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "購物車不存在"
                    };
                }
                await _context.SaveChangesAsync();
                return new ApiResponse
                    {
                        Success = true,
                        Message = "購物車項目已更新"
                    };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = $"更新購物車項目時發生錯誤: " + ex.InnerException?.Message
                };
            }
        }

        [HttpDelete("ClearShoppingCar")]
        public async Task<ApiResponse> ClearShoppingCar(string userName)
        {
            try
            {
                /*if (string.IsNullOrEmpty(userName))
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "請輸入使用者名稱"
                    };
                }*/
                var uId = GetUserIdByName(userName);
                if (GetUserIdByName(userName) == Guid.Empty)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = $"「{userName}」使用者不存在"
                    };
                }
                var car = await _context.ShoppingCar.FirstOrDefaultAsync(c => c.UserId == uId);
                if (car == null)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "購物車不存在"
                    };
                }
                var items = _context.ShoppingCarItems.Where(ci => ci.CarId == car.CarId);
                _context.ShoppingCarItems.RemoveRange(items);
                _context.ShoppingCar.Remove(car);
                await _context.SaveChangesAsync();
                return new ApiResponse
                {
                    Success = true,
                    Message = "購物車已清空"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = $"清空購物車時發生錯誤: " + ex.InnerException?.Message
                };
            }
        }

        [HttpDelete("RemoveItemFromShoppingCar")]
        public async Task<ApiResponse> RemoveItemFromShoppingCar(Guid carItemId)
        {
            try
            {
                var carItem = await _context.ShoppingCarItems.FirstOrDefaultAsync(ci => ci.CarItemId == carItemId);
                if (carItem == null)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "購物車項目不存在"
                    };
                }
                var car = await _context.ShoppingCar.FirstOrDefaultAsync(c => c.CarId == carItem.CarId);
                if (car != null)
                {
                    car.ProductCount -= carItem.Quantity;
                    car.TotalPrice -= carItem.Price;
                }
                _context.ShoppingCarItems.Remove(carItem);
                await _context.SaveChangesAsync();
                return new ApiResponse
                {
                    Success = true,
                    Message = "購物車項目已移除"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = $"移除購物車項目時發生錯誤: " + ex.InnerException?.Message
                };
            }
        }
    }
}

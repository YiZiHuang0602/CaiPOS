using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CaiPOS.Data;
using CaiPOS.Model;
using CaiPOS.ViewModel;

namespace CaiPOS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private DbConn _conn;

        private readonly ILogger<ProductController> _logger;

        private static string CreatNumber()
        {
            Random ran = new Random();
            var s = new StringBuilder();
            s.Append("Food-");
            //for (int i = 0; i < 3; i++)
            //{
            //    s.Append((char)ran.Next('A', 'Z' + 1));
            //}
            for (int i = 0; i < 3; i++)
            {
                s.Append(ran.Next(1, 9));
            }
            return s.ToString();
        }

        public ProductController(ILogger<ProductController> logger, DbConn conn)
        {
            _logger = logger;
            _conn = conn;
        }


        [HttpGet("GetAllProducts")]
        public async Task<List<ProductsDto>> GetAllProducts()
        {
            var products = new List<ProductsDto>();
            var Products = await _conn.Prroducts.ToListAsync();
            foreach(var i in Products)
            {
                products.Add(new ProductsDto
                {
                    Category = i.Category,
                    ProductName = i.ProductName,
                    description = i.description,
                    Price = i.Price,
                });
            }
            return products;
        }
        [HttpGet("GetProductByProductName")]
        public async Task<ApiResponse<ProductsDto>> GetProductByProductName(string productName)
        {
            try
            {
                if (string.IsNullOrEmpty(productName))
                {
                    throw new ArgumentNullException(nameof(productName));
                }
                var Products = await _conn.Prroducts.FirstOrDefaultAsync(s => s.ProductName == productName);
                return Products != null ? 
                    new ApiResponse<ProductsDto> 
                    { 
                        Success = true,
                        Data = new ProductsDto { ProductName = Products.ProductName, Price = Products.Price, description = Products.description } 
                    } 
                    : throw new KeyNotFoundException($"找不到產品：{productName}");
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductsDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        [HttpPost("Postproduct")]
        public async Task<ApiResponse> Postproduct(ProductsDto pDto)
        {
            try
            {
                if(string.IsNullOrEmpty(pDto.Category) || string.IsNullOrEmpty(pDto.ProductName) || pDto.Price.ToString() == "0")
                {
                    throw new ArgumentNullException();
                };
                var products = new Products
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = pDto.ProductName,
                    Price = pDto.Price,
                    description = pDto.description,
                    Category = pDto.Category,
                    ProductNumber = CreatNumber()

                };
                await _conn.Prroducts.AddAsync(products);
                await _conn.SaveChangesAsync();

                return new ApiResponse
                {
                    Success = true,
                    Message = "成功新增",
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                { 
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        [HttpDelete("Delete")]
        public async Task<ApiResponse> Delete(string productName)
        {
            try
            {
                if (string.IsNullOrEmpty(productName))
                {
                    throw new ArgumentNullException(nameof(productName));
                }
                var Products = await _conn.Prroducts.FirstOrDefaultAsync(s => s.ProductName == productName);
                _conn.Prroducts.Remove(Products);
                await _conn.SaveChangesAsync();
                return new ApiResponse
                {
                    Success = true,
                    Message = "刪除成功"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        [HttpPatch("Edit")]
        public async Task<ApiResponse> Edit(string productName, ProductsDto pDto)
        {
            try
            {
                if (string.IsNullOrEmpty(pDto.ProductName))
                {
                    throw new ArgumentNullException(nameof(pDto.ProductName));
                }
                var Products = await _conn.Prroducts.FirstOrDefaultAsync(s => s.ProductName == productName);
                if (Products == null) throw new KeyNotFoundException($"找不到 { Products }");
                Products.Category = pDto.Category;
                Products.description = pDto.description;
                Products.Price = pDto.Price;
                Products.ProductName = pDto.ProductName;
                await _conn.SaveChangesAsync();
                return new ApiResponse
                {
                    Success = true,
                    Message = "編輯成功"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }
    }
}

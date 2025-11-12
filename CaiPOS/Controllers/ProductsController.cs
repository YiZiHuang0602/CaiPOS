using CaiPOS.Data;
using CaiPOS.Models;
using CaiPOS.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.InteropServices;

namespace CaiPOS.Controllers
{

    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly CaiPOSContext _context;

        public ProductsController(CaiPOSContext context)
        {
            _context = context;
        }

        // Get api/products
        [Route("api/[controller]/GetProducts")]
        [HttpGet]
        public async Task<List<ProductDto>> GetProducts()
        {
            var products = new List<ProductDto>();
            var product = await _context.Product.ToListAsync();
            foreach (var i in product)
            {
                var pDto = new ProductDto
                {
                    ProductName = i.ProductName,
                    Category = i.Category,
                    Description = i.Description,
                    Price = i.Price,
                    Status = i.Status
                };
                products.Add(pDto);
            }
            return products;
        }

        [Route("api/[controller]/GetSearchProduct")]
        [HttpGet]
        public async Task<ApiResponse<ProductDto>> GetSearchProduct(string searchProduct)
        {
            try
            {
                var product = await _context.Product.FirstOrDefaultAsync(p => p.ProductName == searchProduct);

                if (product != null)
                {
                    return new ApiResponse<ProductDto>
                    {
                        Success = true,
                        Message = "搜尋成功~",
                        Data = new ProductDto
                        {
                            ProductName = product.ProductName,
                            Category = product.Category,
                            Description = product.Description,
                            Price = product.Price,
                            Status = product.Status
                        }
                    };
                }
                throw new Exception($"找不到{searchProduct}");
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        [Route("api/[controller]/CreateProduct")]
        [HttpPost]
        public async Task<ApiResponse> CreateProduct(ProductDto productDto)
        {
            var foundProduct = await _context.Product.FirstOrDefaultAsync(p => p.ProductName == productDto.ProductName);
            var product = new Product
            {
                ProductName = productDto.ProductName,
                Category = productDto.Category,
                Description = productDto.Description,
                Price = productDto.Price,
                Status = productDto.Status
            };

            if (foundProduct == null)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return new ApiResponse
                {
                    Success = true,
                    Message = "新增成功!",
                };
            }
            return new ApiResponse
            {
                Success = true,
                Message = "新增失敗!!",
            };
        }

        [Route("api/[controller]/EditProduct")]
        [HttpPatch]
        public async Task<ApiResponse> EditProduct(string search, ProductDto productDto)
        {
            try
            {
                var foundProduct = await _context.Product.FirstOrDefaultAsync(p => p.ProductName == search);
                if (foundProduct == null) throw new Exception($"找不到 {search}");
                foundProduct.ProductName = productDto.ProductName;
                foundProduct.Category = productDto.Category;
                foundProduct.Description = productDto.Description;
                foundProduct.Price = productDto.Price;
                foundProduct.Status = productDto.Status;
                await _context.SaveChangesAsync();
                return new ApiResponse
                {
                    Success = true,
                    Message = "修改成功",
                };

            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        [Route("api/[controller]/DeleteProduct")]
        [HttpDelete]
        public async Task<ApiResponse> DeleteProduct(string productName)
        {
            try
            {
                var foundProduct = await _context.Product.FirstOrDefaultAsync(p => p.ProductName == productName);
                if (foundProduct == null) throw new Exception($"找不到{productName}");

                _context.Product.Remove(foundProduct);
                await _context.SaveChangesAsync();
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
                    Message = ex.Message
                };
            }

        }

    }
}
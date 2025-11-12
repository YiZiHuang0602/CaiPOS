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
        public async Task<Boolean> CreateProduct(Product product)
        {
            var foundProduct = await _context.Product.FirstOrDefaultAsync(p => p.ProductName == product.ProductName);
            if (foundProduct == null)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        [Route("api/[controller]/EditProduct")]
        [HttpPatch]
        public async Task<IActionResult> EditProduct(Product product)
        {
            var foundProduct = await _context.Product.FirstOrDefaultAsync(p => p.ProductName == product.ProductName);
            if (foundProduct != null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(GetProducts));
            }
            return BadRequest();
        }

        [Route("api/[controller]/DeleteProduct")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(Product product)
        {
            var foundProduct = await _context.Product.FirstOrDefaultAsync(p => p.ProductName == product.ProductName);
            if (foundProduct == null)
            {
                return NotFound();
            }

            var productToDelete = await _context.Product.FindAsync(product.ProductId);
            if (productToDelete != null)
            {
                _context.Product.Remove(productToDelete);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetProducts));
        }

        private bool ProductExists(Guid id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
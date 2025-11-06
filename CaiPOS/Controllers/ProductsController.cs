using CaiPOS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CaiPOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // Get api/products
        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            Product[] products = new Product[]
            {
            new Product { ProductName = "切仔麵", Category = "主食(麵類)", Description = "", price = 45, Status = "販賣中"},

            };
        }
    }
}

using Dapper_StoredProcedures.Application.Services;
using Dapper_StoredProcedures.Application.Services.Abtractions;
using Dapper_StoredProcedures.Application.Services.Abtractions.Abtractions;
using Dapper_StoredProcedures.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dapper_StoredProcedures.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("category-sold")]
        public async Task<IActionResult> GetWithCategorySold()
        {
            var products = await _productService.GetProductsWithCategoryAndSold();
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            try
            {
                var id = await _productService.CreateProduct(product);
                return Ok(new { Id = id });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}

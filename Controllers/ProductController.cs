using Dapper_StoredProcedures.Application.DTOs.Requests;
using Dapper_StoredProcedures.Application.Services;
using Dapper_StoredProcedures.Application.Services.Abtractions;
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

        [HttpGet("summaries")]
        public async Task<IActionResult> GetProductSummaries(
              [FromQuery] int? categoryId = null,
             [FromQuery] string? sku = null,
             [FromQuery] string sortBy = "SoldDisplay",
             [FromQuery] string sortDirection = "DESC")
        {
            var products = await _productService.GetProductSummaries(
                categoryId,
                sku,
                sortBy,
                sortDirection
            );
            return Ok(products);
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct(CreateProductRequest product)
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
        [HttpGet("with-category")]
        public async Task<IActionResult> GetProductsWithCategory()
        {
            var result = await _productService.GetProductsWithCategory();
            return Ok(result);
        }
    }
}

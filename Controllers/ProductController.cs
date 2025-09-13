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

        [HttpGet("filter-sort")]
        public async Task<IActionResult> GetProductsByFilterAndSort(
              [FromQuery] int? categoryId = null,
             [FromQuery] string? sku = null,
             [FromQuery] string sortBy = "SoldDisplay",
             [FromQuery] string sortDirection = "DESC",
              [FromQuery] int pageIndex = 1,
             [FromQuery] int pageSize = 10)
        {
            var products = await _productService.GetProductsByFilterAndSort(
                categoryId,
                sku,
                sortBy,
                sortDirection,
                pageIndex,
                pageSize
            );
            return Ok(products);
        }


        [HttpPost]
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

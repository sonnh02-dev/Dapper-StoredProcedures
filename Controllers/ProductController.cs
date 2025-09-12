using Dapper_StoredProcedures.Entities;
using Dapper_StoredProcedures.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Dapper_StoredProcedures.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductRepository _repository;

    public ProductsController(ProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("with-category")]
    public async Task<IActionResult> GetWithCategory()
    {
        var products = await _repository.GetProductsWithCategoryAsync();
        return Ok(products);
    }

    [HttpPost("category")]
    public async Task<IActionResult> CreateCategory(Category category)
    {
        var newId = await _repository.InsertCategoryAsync(category);
        return Ok(new { Id = newId });
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        var newId = await _repository.InsertProductAsync(product);
        return Ok(new { Id = newId });
    }
}

using Dapper_StoredProcedures.Domain.Entities;

namespace Dapper_StoredProcedures.Application.Services.Abtractions.Abtractions
{
    public interface IProductService
    {
        Task<int> CreateProduct(Product product);
        Task<IEnumerable<Product>> GetProductsWithCategoryAndSold();
    }
}

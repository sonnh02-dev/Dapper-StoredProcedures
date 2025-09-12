using Dapper_StoredProcedures.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper_StoredProcedures.Domain.IRepositories
{
    public interface IProductRepository
    {
        Task<int> InsertProduct(Product product);
        Task<bool> IsProductNameExists(string name);
        Task<IEnumerable<Product>> GetProductsWithCategoryAndSold();
    }
}

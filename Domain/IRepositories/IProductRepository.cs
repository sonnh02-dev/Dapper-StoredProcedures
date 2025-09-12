using Dapper_StoredProcedures.Application.DTOs.Requests;
using Dapper_StoredProcedures.Application.DTOs.Responses;
using Dapper_StoredProcedures.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper_StoredProcedures.Domain.IRepositories
{
    public interface IProductRepository
    {
        Task<int> InsertProduct(CreateProductRequest  product);
        Task<bool> IsProductNameExists(string name);
        Task<IEnumerable<ProductSummaryResponse>> GetProductSummaries(
             int? categoryId = null,
             string? sku = null,
             string sortBy = "SoldDisplay",
             string sortDirection = "DESC");
        Task<IEnumerable<Product>> GetProductsWithCategory();

    }
}

using Dapper_StoredProcedures.Application.DTOs.Requests;
using Dapper_StoredProcedures.Application.DTOs.Responses;
using Dapper_StoredProcedures.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Dapper_StoredProcedures.Application.Services.Abtractions
{
    public interface IProductService
    {
        Task<int> CreateProduct(CreateProductRequest product);
        Task<IEnumerable<ProductStatsResponse>> GetProductsByFilterAndSort(
          int? categoryId = null,
         string? sku = null,
         string sortBy = "SoldDisplay",
         string sortDirection = "DESC",
          int pageIndex = 1,
         int pageSize = 10);
        Task<IEnumerable<Product>> GetProductsWithCategory();

    }
}

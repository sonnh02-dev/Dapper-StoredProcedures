using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Dapper_StoredProcedures.Application.DTOs.Requests;
using Dapper_StoredProcedures.Application.DTOs.Responses;
using Dapper_StoredProcedures.Domain.Entities;
using Dapper_StoredProcedures.Domain.IRepositories;
using Dapper_StoredProcedures.Infrastructure.Persistence;

namespace Dapper_StoredProcedures.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public ProductRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<bool> IsProductNameExists(string name)
        {
            var sql = @"
           SELECT CASE WHEN EXISTS (
              SELECT 1 
              FROM Products 
              WHERE Name = @Name
          ) THEN 1 ELSE 0 END";

            using var conn = _dbFactory.OpenConnection();
            return await conn.ExecuteScalarAsync<bool>(sql, new { Name = name });
        }
        public async Task<IEnumerable<Product>> GetProductsWithCategory()
        {
            var sql = @"
            SELECT p.Id, p.Name, p.Price, p.Quantity,p.CategoryId,c.Id, c.Name
            FROM Products p
             INNER JOIN Categories c ON p.CategoryId = c.Id";

            using var conn = _dbFactory.OpenConnection();

            var products = await conn.QueryAsync<Product, Category, Product>(
                sql,
                (product, category) =>
                {
                    product.Category = category;
                    return product;
                },
                splitOn: "Id"  // báo Dapper cột bắt đầu map sang Category
            );

            return products;
        }



        public async Task<IEnumerable<ProductStatsResponse>> GetProductsByFilterAndSort(
         int? categoryId = null,
         string? sku = null,
         string sortBy = "SoldDisplay",
         string sortDirection = "DESC",
         int pageIndex=1,
         int pageSize=10)
        {
            using var conn = _dbFactory.OpenConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", categoryId);
            parameters.Add("@SKU", sku);
            parameters.Add("@SortBy", sortBy);
            parameters.Add("@SortDirection", sortDirection);
            parameters.Add("@PageIndex", pageIndex);
            parameters.Add("@PageSize", pageSize);

            var result = await conn.QueryAsync<ProductStatsResponse>(
                "sp_GetProductsByFilterAndSort",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }



        public async Task<int> InsertProduct(CreateProductRequest product)
        {
            var sql = @"INSERT INTO Products (Name, Price, Quantity, CategoryId, SKU) 
                VALUES (@Name, @Price, @Quantity, @CategoryId, @SKU); 
                SELECT CAST(SCOPE_IDENTITY() as int)";
            using var conn = _dbFactory.OpenConnection();
            return await conn.ExecuteScalarAsync<int>(sql, product);
        }

    }
}

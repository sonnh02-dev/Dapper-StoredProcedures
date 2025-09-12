using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
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



        public async Task<IEnumerable<Product>> GetProductsWithCategoryAndSold()
        {
            using var conn = _dbFactory.OpenConnection();
            return await conn.QueryAsync<Product>(
                "sp_GetProductsWithCategoryAndSold",
                commandType: System.Data.CommandType.StoredProcedure
            );
        }

       
        public async Task<int> InsertProduct(Product product)
        {
            var sql = @"INSERT INTO Products (Name, Price, Quantity, CategoryId) 
                        VALUES (@Name, @Price, @Quantity, @CategoryId); 
                        SELECT CAST(SCOPE_IDENTITY() as int)";
            using var conn = _dbFactory.OpenConnection();
            return await conn.ExecuteScalarAsync<int>(sql, product);
        }
    }
}

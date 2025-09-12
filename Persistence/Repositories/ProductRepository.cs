using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper_StoredProcedures.Entities;
using Dapper;

namespace Dapper_StoredProcedures.Persistence.Repositories
{
    public class ProductRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public ProductRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IEnumerable<Product>> GetProductsWithCategoryAsync()
        {
            var sql = @"
                SELECT p.Id, p.Name, p.Price, p.Quantity, 
                       p.CategoryId, c.Name as CategoryName
                FROM Products p
                INNER JOIN Categories c ON p.CategoryId = c.Id";

            using var conn = _dbFactory.OpenConnection();
            return await conn.QueryAsync<Product>(sql);
        }
        public async Task<IEnumerable<Product>> GetProductsWithCategory_SPAsync()
        {
            using var conn = _dbFactory.OpenConnection();
            return await conn.QueryAsync<Product>(
                "sp_GetProductsWithCategory",
                commandType: System.Data.CommandType.StoredProcedure
            );
        }

        public async Task<int> InsertCategoryAsync(Category category)
        {
            var sql = "INSERT INTO Categories (Name) VALUES (@Name); SELECT CAST(SCOPE_IDENTITY() as int)";
            using var conn = _dbFactory.OpenConnection();
            return await conn.ExecuteScalarAsync<int>(sql, category);
        }

        public async Task<int> InsertProductAsync(Product product)
        {
            var sql = @"INSERT INTO Products (Name, Price, Quantity, CategoryId) 
                        VALUES (@Name, @Price, @Quantity, @CategoryId); 
                        SELECT CAST(SCOPE_IDENTITY() as int)";
            using var conn = _dbFactory.OpenConnection();
            return await conn.ExecuteScalarAsync<int>(sql, product);
        }
    }
}

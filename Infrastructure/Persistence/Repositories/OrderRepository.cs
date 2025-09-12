using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using Dapper_StoredProcedures.Domain.Entities;
using Dapper_StoredProcedures.Infrastructure.Persistence;

namespace Dapper_StoredProcedures.Infrastructure.Persistence.Repositories
{
    public class OrderRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public OrderRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        // Insert Customer
        public async Task<int> InsertCustomerAsync(Customer customer)
        {
            var sql = @"INSERT INTO Customers (Name, Email) 
                        VALUES (@Name, @Email); 
                        SELECT CAST(SCOPE_IDENTITY() as int)";
            using var conn = _dbFactory.OpenConnection();
            return await conn.ExecuteScalarAsync<int>(sql, customer);
        }

        // Insert Order
        public async Task<int> InsertOrderAsync(Order order)
        {
            var sql = @"INSERT INTO Orders (CustomerId, OrderDate, TotalAmount) 
                        VALUES (@CustomerId, @OrderDate, @TotalAmount); 
                        SELECT CAST(SCOPE_IDENTITY() as int)";
            using var conn = _dbFactory.OpenConnection();
            return await conn.ExecuteScalarAsync<int>(sql, order);
        }

        // Get Orders with Customer join
        public async Task<IEnumerable<Order>> GetOrdersWithCustomerAsync()
        {
            var sql = @"
                SELECT o.Id, o.CustomerId, o.OrderDate, o.TotalAmount,
                       c.Name AS CustomerName
                FROM Orders o
                INNER JOIN Customers c ON o.CustomerId = c.Id";
            using var conn = _dbFactory.OpenConnection();
            return await conn.QueryAsync<Order>(sql);
        }

        // Stored Procedure 
        public async Task<IEnumerable<Order>> GetOrdersWithCustomer_SPAsync()
        {
            using var conn = _dbFactory.OpenConnection();
            return await conn.QueryAsync<Order>(
                "sp_GetOrdersWithCustomer",
                commandType: CommandType.StoredProcedure
            );
        }
    }
}

using Dapper;
using Dapper_StoredProcedures.Entities;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Dapper_StoredProcedures.Persistence.Repositories
{
    public class OrderItemRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public OrderItemRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        // Insert OrderItem
        public async Task<int> InsertOrderItemAsync(OrderItem item)
        {
            var sql = @"INSERT INTO OrderItems (OrderId, ProductId, Quantity)
                        VALUES (@OrderId, @ProductId, @Quantity)";
            using var conn = _dbFactory.OpenConnection();
            return await conn.ExecuteAsync(sql, item);
        }

        // Get OrderItems with Product info (join)
        public async Task<IEnumerable<OrderItem>> GetOrderItemsWithProductAsync(int orderId)
        {
            var sql = @"
                SELECT oi.OrderId, oi.ProductId, oi.Quantity, 
                       p.Name AS ProductName, p.Price
                FROM OrderItems oi
                INNER JOIN Products p ON oi.ProductId = p.Id
                WHERE oi.OrderId = @OrderId";

            using var conn = _dbFactory.OpenConnection();
            return await conn.QueryAsync<OrderItem>(sql, new { OrderId = orderId });
        }

        // Stored Procedure example
        public async Task<IEnumerable<OrderItem>> GetOrderItemsWithProduct_SPAsync(int orderId)
        {
            using var conn = _dbFactory.OpenConnection();
            return await conn.QueryAsync<OrderItem>(
                "sp_GetOrderItemsWithProduct",
                new { OrderId = orderId },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}

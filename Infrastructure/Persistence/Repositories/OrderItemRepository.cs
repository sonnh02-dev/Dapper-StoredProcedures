using Dapper;
using Dapper_StoredProcedures.Domain.Entities;
using Dapper_StoredProcedures.Domain.IRepositories;
using Dapper_StoredProcedures.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Dapper_StoredProcedures.Infrastructure.Persistence.Repositories
{
    public class OrderItemRepository:IOrderItemRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public OrderItemRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public async Task<bool> IsDuplicateOrderItem(int orderId, int productId)
        {
            var sql = @"SELECT CASE WHEN EXISTS (
                            SELECT 1 FROM OrderItems 
                            WHERE OrderId = @OrderId AND ProductId = @ProductId
                        ) THEN 1 ELSE 0 END";
            using var conn = _dbFactory.OpenConnection();
            return await conn.ExecuteScalarAsync<bool>(sql, new { OrderId = orderId, ProductId = productId });
        }
        public async Task<int> InsertOrderItem(OrderItem item)
        {
            var sql = @"INSERT INTO OrderItems (OrderId, ProductId, Quantity)
                        VALUES (@OrderId, @ProductId, @Quantity)";
            using var conn = _dbFactory.OpenConnection();
            return await conn.ExecuteAsync(sql, item);
        }
     

        public async Task<IEnumerable<OrderItem>> GetOrderItemsWithProduct(int orderId)
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

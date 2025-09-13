using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using Dapper_StoredProcedures.Domain.Entities;
using Dapper_StoredProcedures.Infrastructure.Persistence;
using Dapper_StoredProcedures.Domain.IRepositories;

namespace Dapper_StoredProcedures.Infrastructure.Persistence.Repositories
{
    public class OrderRepository:IOrderRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public OrderRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }


        //Raw query
        public async Task<int> InsertOrder(Order order)
        {
            var sql = @"INSERT INTO Orders (CustomerId, OrderDate, TotalAmount) 
                        VALUES (@CustomerId, @OrderDate, @TotalAmount); 
                        SELECT CAST(SCOPE_IDENTITY() as int)";
            using var conn = _dbFactory.OpenConnection();
            return await conn.ExecuteScalarAsync<int>(sql, order);
        }



        // Stored Procedure 
        public async Task<IEnumerable<Order>> GetOrdersByFilterAndSort(
          string? paymentStatus,
          string? sortBy,
          string? sortDirection,
          string? productName,
          int? customerId
      )
        {
            using var conn = _dbFactory.OpenConnection();
            return await conn.QueryAsync<Order>(
                "sp_GetOrdersWithCustomer",
                new
                {
                    PaymentStatus = paymentStatus,
                    SortBy = sortBy,
                    SortDirection = sortDirection,
                    ProductName = productName,
                    CustomerId = customerId
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}

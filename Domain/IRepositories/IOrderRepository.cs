using Dapper_StoredProcedures.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper_StoredProcedures.Domain.IRepositories
{
    public interface IOrderRepository
    {
        Task<int> InsertOrder(Order order);
        Task<IEnumerable<Order>> GetOrdersByFilterAndSort(
              string? paymentStatus,
              string? sortBy,
              string? sortDirection,
              string? productName,
              int? customerId
          );
    }
}

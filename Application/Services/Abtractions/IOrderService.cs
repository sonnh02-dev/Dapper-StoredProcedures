using Dapper_StoredProcedures.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper_StoredProcedures.Application.Services.Abtractions
{
    public interface IOrderService
    {
        Task<int> CreateOrder(Order order);
        Task<IEnumerable<Order>> GetOrdersByFilterAndSort(
               string? paymentStatus,
               string? sortBy,
               string? sortDirection,
               string? productName,
               int? customerId
           );
    }
}

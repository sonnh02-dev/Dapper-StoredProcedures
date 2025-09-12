using Dapper_StoredProcedures.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper_StoredProcedures.Domain.IRepositories
{
    public interface IOrderRepository
    {
        Task<int> InsertCustomer(Customer customer);
        Task<int> InsertOrder(Order order);
        Task<IEnumerable<Order>> GetOrdersWithCustomer();
        Task<IEnumerable<Order>> GetOrdersWithCustomer_SP();
    }
}

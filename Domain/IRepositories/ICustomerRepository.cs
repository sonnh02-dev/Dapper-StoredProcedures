using Dapper_StoredProcedures.Application.DTOs.Responses;
using Dapper_StoredProcedures.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper_StoredProcedures.Domain.IRepositories
{
    public interface ICustomerRepository
    {
        Task<int> InsertCustomer(Customer customer);
        Task<bool> IsCustomerExists(string name);
        Task<IEnumerable<CustomerStatsResponse>> GetCustomersStats();
    }
}

using Dapper_StoredProcedures.Application.DTOs.Requests;
using Dapper_StoredProcedures.Application.DTOs.Responses;
using Dapper_StoredProcedures.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper_StoredProcedures.Domain.IRepositories
{
    public interface ICustomerRepository
    {
        Task<int> InsertCustomer(CreateCustomerRequest customer);
        Task<bool> IsCustomerExists(int? id = null, string? email = null);
        Task<int> UpdateCustomer(Customer newData);

        Task<IEnumerable<CustomerStatsResponse>> GetCustomersStats();
       
    }
}

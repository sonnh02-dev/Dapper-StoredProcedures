using Dapper_StoredProcedures.Application.DTOs.Responses;
using Dapper_StoredProcedures.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper_StoredProcedures.Application.Services.Abtractions
{
    public interface ICustomerService
    {
        Task<int> CreateCustomer(Customer customer);
        Task<IEnumerable<CustomerSummaryResponse>> GetCustomerSummaries();
    }
}

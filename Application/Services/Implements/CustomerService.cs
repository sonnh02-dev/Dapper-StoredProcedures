using Dapper_StoredProcedures.Application.DTOs.Responses;
using Dapper_StoredProcedures.Application.Services.Abtractions;
using Dapper_StoredProcedures.Domain.Entities;
using Dapper_StoredProcedures.Domain.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper_StoredProcedures.Application.Services.Implements
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<int> CreateCustomer(Customer customer)
        {
            bool exists = await _customerRepository.IsCustomerExists(customer.Name);
            if (exists)
            {
                throw new Exception("Customer already exists");
            }

            return await _customerRepository.InsertCustomer(customer);
        }




        public Task<IEnumerable<CustomerStatsResponse>> GetCustomersStats()
        {
            return _customerRepository.GetCustomersStats();
        }
    }
}

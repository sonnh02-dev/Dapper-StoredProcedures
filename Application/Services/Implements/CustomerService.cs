using AutoMapper;
using Dapper_StoredProcedures.Application.DTOs.Requests;
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
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateCustomer(CreateCustomerRequest customer)
        {
            bool exists = await _customerRepository.IsCustomerExists(email: customer.Email);
            if (exists)
            {
                throw new Exception("Customer already exists");
            }

            return await _customerRepository.InsertCustomer(customer);
        }


        public async Task<bool> UpdateCustomer(int id, UpdateCustomerRequest request)
        {
            var isExists = await _customerRepository.IsCustomerExists(id);
            if (!isExists) return false;

            var customer = _mapper.Map<Customer>(request);
            customer.Id = id;

            var rows = await _customerRepository.UpdateCustomer(customer);
            return rows > 0;
        }


        public Task<IEnumerable<CustomerStatsResponse>> GetCustomersStats()
        {
            return _customerRepository.GetCustomersStats();
        }
    }
}

using Dapper_StoredProcedures.Application.Services.Abtractions;
using Dapper_StoredProcedures.Domain.Entities;
using Dapper_StoredProcedures.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Dapper_StoredProcedures.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
            var id = await _customerService.CreateCustomer(customer);
            return Ok(new { Id = id });
        } 
        [HttpGet("stats")]
        public async Task<IActionResult> GetCustomersStats()
        {
            var cusStats = await _customerService.GetCustomersStats();
            return Ok(cusStats);
        }
    }
}

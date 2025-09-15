using Dapper_StoredProcedures.Application.DTOs.Requests;
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
        [HttpPut("{customerId}")]
        public async Task<IActionResult> Update(int customerId, [FromBody] UpdateCustomerRequest request)
        {
            var success = await _customerService.UpdateCustomer(customerId,request);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerRequest request)
        {
            var id = await _customerService.CreateCustomer(request);
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

using Dapper_StoredProcedures.Entities;
using Dapper_StoredProcedures.Persistence;
using Dapper_StoredProcedures.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Dapper_StoredProcedures.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly OrderRepository _orderRepo;

        public CustomerController(OrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
            var id = await _orderRepo.InsertCustomerAsync(customer);
            return Ok(new { Id = id });
        }
    }
}

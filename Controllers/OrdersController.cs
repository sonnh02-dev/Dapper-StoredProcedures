using Dapper_StoredProcedures.Application.Services;
using Dapper_StoredProcedures.Application.Services.Abtractions;
using Dapper_StoredProcedures.Application.Services.Abtractions.Abtractions;
using Dapper_StoredProcedures.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dapper_StoredProcedures.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;

        public OrdersController(IOrderService orderService, IOrderItemService orderItemService)
        {
            _orderService = orderService;
            _orderItemService = orderItemService;
        }

       

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            var id = await _orderService.CreateOrder(order);
            return Ok(new { Id = id });
        }

        [HttpPost("item")]
        public async Task<IActionResult> CreateOrderItem(OrderItem item)
        {
            var rows = await _orderItemService.CreateOrderItem(item);
            return Ok(new { RowsAffected = rows });
        }

        [HttpGet("with-customer")]
        public async Task<IActionResult> GetOrdersWithCustomer()
        {
            var orders = await _orderService.GetOrdersWithCustomer();
            return Ok(orders);
        }

        [HttpGet("item/{orderId}")]
        public async Task<IActionResult> GetOrderItems(int orderId)
        {
            var items = await _orderItemService.GetOrderItemsWithProduct(orderId);
            return Ok(items);
        }
    }
}

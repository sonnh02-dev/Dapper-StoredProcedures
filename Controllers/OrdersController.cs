using Dapper_StoredProcedures.Application.Services;
using Dapper_StoredProcedures.Application.Services.Abtractions;
using Dapper_StoredProcedures.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace Dapper_StoredProcedures.Controllers;

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
        return CreatedAtAction(nameof(GetOrderItems), new { orderId = id }, new { Id = id });
    }

    [HttpPost("{orderId}/items")]
    public async Task<IActionResult> CreateOrderItem(int orderId, OrderItem item)
    {
        item.OrderId = orderId; 
        var rows = await _orderItemService.CreateOrderItem(item);
        return Ok(new { RowsAffected = rows });
    }

    [HttpGet("by-filter-sort")]
    public async Task<IActionResult> GetOrdersByFilterAndSort(
        [FromQuery] string? paymentStatus,
        [FromQuery] string? sortBy = "OrderDate",
        [FromQuery] string? sortDirection = "DESC",
        [FromQuery] string? productName = null,
        [FromQuery] int? customerId = null
    )
    {
        var orders = await _orderService.GetOrdersByFilterAndSort(
            paymentStatus,
            sortBy,
            sortDirection,
            productName,
            customerId
        );

        return Ok(orders);
    }

    [HttpGet("{orderId}/items")]
    public async Task<IActionResult> GetOrderItems(int orderId)
    {
        var items = await _orderItemService.GetOrderItemsWithProduct(orderId);
        return Ok(items);
    }
}

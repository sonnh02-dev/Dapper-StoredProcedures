using Dapper_StoredProcedures.Domain.Entities;

namespace Dapper_StoredProcedures.Application.Services.Abtractions.Abtractions
{
    public interface IOrderItemService
    {
        public Task<int> CreateOrderItem(OrderItem item);
        public Task<IEnumerable<OrderItem>> GetOrderItemsWithProduct(int orderId);

    }
}

using Dapper_StoredProcedures.Domain.Entities;

namespace Dapper_StoredProcedures.Domain.IRepositories
{
    public interface IOrderItemRepository
    {
        Task<int> InsertOrderItem(OrderItem item);
        Task<IEnumerable<OrderItem>> GetOrderItemsWithProduct(int orderId);
        Task<bool> IsDuplicateOrderItem(int orderId, int productId);

    }
}

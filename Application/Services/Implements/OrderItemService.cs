using Dapper_StoredProcedures.Domain.Entities;
using Dapper_StoredProcedures.Domain.IRepositories;
using Dapper_StoredProcedures.Application.Services.Abtractions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper_StoredProcedures.Application.Services.Abtractions.Abtractions;

namespace Dapper_StoredProcedures.Application.Services.Implements
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public async Task<int> CreateOrderItem(OrderItem item)
        {
            bool exists = await _orderItemRepository.IsDuplicateOrderItem(item.OrderId, item.ProductId);
            if (exists)
            {
                throw new InvalidOperationException("OrderItem already exists for this Order and Product.");
            }

            return await _orderItemRepository.InsertOrderItem(item);
        }

        public Task<IEnumerable<OrderItem>> GetOrderItemsWithProduct(int orderId)
        {
            return _orderItemRepository.GetOrderItemsWithProduct(orderId);
        }

      
    }
}

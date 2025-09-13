using Dapper_StoredProcedures.Application.Services.Abtractions;
using Dapper_StoredProcedures.Domain.Entities;
using Dapper_StoredProcedures.Domain.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper_StoredProcedures.Application.Services.Implements
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

       

        public Task<int> CreateOrder(Order order)
        {
            return _orderRepository.InsertOrder(order);
        }

        public async Task<IEnumerable<Order>> GetOrdersByFilterAndSort(
          string? paymentStatus,
          string? sortBy,
          string? sortDirection,
          string? productName,
          int? customerId
      )
        {
            return await _orderRepository.GetOrdersByFilterAndSort(
                paymentStatus,
                sortBy,
                sortDirection,
                productName,
                customerId
            );
        }


    }
}

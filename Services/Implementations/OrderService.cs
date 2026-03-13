using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class OrderService(IOrderRepository orderRepository) : IOrderService
{
    private IOrderRepository _orderRepository = orderRepository;
    
    public void CreateOrder(RequestOrderDto orderDto)
    {
        if (GetOrderById(orderDto.Id) != null)//TODO: to fix this "Expression is always true according to nullable reference types' annotations"
        {
            Order order = new Order
            {
                Id = orderDto.Id,
                CustomerName = orderDto.CustomerName,
                DeliveryAddress = orderDto.DeliveryAddress,
                CreatedAt = orderDto.CreatedAt,
                Status = orderDto.Status,
            };
            _orderRepository.Create(order);
        }
    }

    public void CancelOrder(Guid orderId)
    {
        if (_orderRepository.Read() != null)
        {
            if (GetOrderById(orderId).Result.Status == OrderStatus.Created || GetOrderById(orderId).Result.Status == OrderStatus.Processing)
            {
                UpdateOrderStatus(OrderStatus.Cancelled, orderId);
            }

            //GetOrderById(orderId).Status = OrderStatus.Cancelled;//TODO: to change to this status
        }
    }
    
    public async Task<Order> GetOrderById(Guid orderId)
    {
        var orders = await _orderRepository.Read();
        
        var findBook = SearchById(orderId, orders);
        return findBook;
    }
    
    public Order SearchById(Guid orderId, List<Order> orders)
    {
        var findOrder = orders.Find(order => order.Id == orderId);
        return findOrder;
    }
    
    public void UpdateOrderStatus(OrderStatus orderStatus, Guid Id)
    {
        _orderRepository.UpdateOrder(orderStatus, Id, (order, status) => { order.Status = status;});
    }

}
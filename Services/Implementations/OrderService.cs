using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;
using FulfillmentCenter.Services.UpdateOrderStatus;

namespace FulfillmentCenter.Services.Implementations;

public class OrderService(IOrderRepository orderRepository, IShipmentRepository shipmentRepository) : IOrderService
{
    private IOrderRepository _orderRepository = orderRepository;
    private IShipmentRepository _shipmentRepository = shipmentRepository;
    
    private OrderHandlerFactory _orderHandlerFactory = new OrderHandlerFactory();
    
    public async Task CreateOrder(RequestOrderDto orderDto)
    {
        /*if (GetOrderById(orderDto.Id) != null)//TODO: to fix this "Expression is always true according to nullable reference types' annotations"
        {
            Order order = new Order
            {
                Id = orderDto.Id,
                CustomerName = orderDto.CustomerName,
                DeliveryAddress = orderDto.DeliveryAddress,
                CreatedAt = orderDto.CreatedAt,
                Status = orderDto.Status,
                //TODO: to add shippment here, by finding it in db
            };
            _orderRepository.Create(order);
        }*/
        if (orderDto.Status != OrderStatus.Created)
        {
            throw new ArgumentException("first status of order should be Created");
        }
        Order order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = orderDto.CustomerId,
            DeliveryAddress = orderDto.DeliveryAddress,
            //CreatedAt = DateTime.SpecifyKind(orderDto.CreatedAt, DateTimeKind.Unspecified),
            Status = OrderStatus.Created
            //TODO: to add shippment here, by finding it in db
        };
        await _orderRepository.Create(order);
    }

    public async Task CancelOrder(Guid orderId)
    {
        var orderToCancelStatus = (await GetOrderById(orderId)).Status;
            if (orderToCancelStatus == OrderStatus.Created || orderToCancelStatus == OrderStatus.Processing)
            {
                var service = _orderHandlerFactory.GetHandler(orderToCancelStatus);
                await service.HandleAsync(orderId);
            }
            //GetOrderById(orderId).Status = OrderStatus.Cancelled;//TODO: to change to this status
    }
    
    public async Task<Order> GetOrderById(Guid orderId)
    {
        var orders = await _orderRepository.Read();
        
        var findBook = SearchById(orderId, orders);
        return findBook;
    }
    
    private Order SearchById(Guid orderId, List<Order> orders)
    {
        var findOrder = orders.FirstOrDefault(order => order.Id == orderId);
        if (findOrder != null)
        {
            return findOrder;
        }

        throw new ArgumentNullException(nameof(orderId), "Order not found");
    }
}
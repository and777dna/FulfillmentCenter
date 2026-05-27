using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;
using FulfillmentCenter.Services.UpdateOrderStatus;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Memory;

namespace FulfillmentCenter.Services.Implementations;

public class OrderService : IOrderService
{
    private IOrderRepository _orderRepository;
    private IShipmentRepository _shipmentRepository;
    private readonly ICacheService _cache;

    private Lazy<Task<List<Order>>> _orders;
    
    private OrderHandlerFactory _orderHandlerFactory = new OrderHandlerFactory();

    public OrderService(IOrderRepository orderRepository, IShipmentRepository shipmentRepository, ICacheService cache)
    {
        //ResetCache();
        _orderRepository = orderRepository;
        _shipmentRepository = shipmentRepository;
        _cache = cache;
    }
    
    /*private void ResetCache()
    {
        _orders = new Lazy<Task<List<Order>>>(() => _orderRepository.Read());
    }*/
    
    public async Task CreateOrder(RequestOrderDto orderDto, string idempotencyKey)
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
        if (_cache.TryGet<Guid>(idempotencyKey, out var cachedOrderId))
        {
            return;
        }
        
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
        
        _cache.Set(idempotencyKey, order.Id, TimeSpan.FromMinutes(10));
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
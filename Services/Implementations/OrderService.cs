using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class OrderService(IOrderRepository orderRepository, IShipmentRepository shipmentRepository) : IOrderService
{
    private IOrderRepository _orderRepository = orderRepository;
    private IShipmentRepository _shipmentRepository = shipmentRepository;
    
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
            CustomerName = orderDto.CustomerName,
            DeliveryAddress = orderDto.DeliveryAddress,
            CreatedAt = orderDto.CreatedAt,
            //CreatedAt = DateTime.SpecifyKind(orderDto.CreatedAt, DateTimeKind.Unspecified),
            Status = orderDto.Status,
            //TODO: to add shippment here, by finding it in db
        };
        await _orderRepository.Create(order);
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
        var findOrder = orders.FirstOrDefault(order => order.Id == orderId);
        if (findOrder != null)
        {
            return findOrder;
        }

        throw new ArgumentNullException();
    }
    
    public async Task UpdateOrderStatus(OrderStatus orderStatus, Guid Id)
    {
        switch (orderStatus)
        {
            //case OrderStatus.ReadyToShip: shipmentRepository.Create(); return;//TODO: to create shipment;
            case OrderStatus.Delivered: 
                await _orderRepository.UpdateOrder(orderStatus, Id, (order, status) => { order.Status = status;});
                await _orderRepository.Delete(Id); 
                return;//TODO: to delete order with soft delete
            case OrderStatus.Cancelled: 
                await _orderRepository.UpdateOrder(orderStatus, Id, (order, status) => { order.Status = status;});
                await _orderRepository.Delete(Id);
                //await _shipmentRepository.UpdateShipmentStatus(,ShipmentStatus.Cancelled);
                return;//TODO: to delete order && to delete shipment if exist
            case OrderStatus.Processing: 
                await _orderRepository.UpdateOrder(orderStatus, Id, (order, status) => { order.Status = status;});
                return;
            case OrderStatus.Created: throw new ArgumentException("order has been already Created.");}
    }

}
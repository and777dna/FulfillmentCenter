using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;

namespace FulfillmentCenter.Services.Interfaces;

public interface IOrderService
{
    public Task CreateOrder(RequestOrderDto orderDto);
    public void CancelOrder(Guid orderId);
    public Task UpdateOrderStatus(OrderStatus status,Guid id);
    public Task<Order> GetOrderById(Guid orderId);

    public Order SearchById(Guid orderId, List<Order> orders);
}
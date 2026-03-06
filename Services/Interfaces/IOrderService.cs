using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;

namespace FulfillmentCenter.Services.Interfaces;

public interface IOrderService
{
    public void CreateOrder(Order order);
    public void CancelOrder(Guid orderId);
    public void UpdateOrderStatus(OrderStatus status,Guid id);

    public Order GetOrderById(Guid orderId);

    public Order SearchById(Guid orderId, List<Order> orders);
}
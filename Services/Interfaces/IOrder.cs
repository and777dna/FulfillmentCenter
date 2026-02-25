using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Services.Interfaces;

public interface IOrder
{
    public void CreateOrder(Order order);
    public void CancelOrder(Guid orderId);
}
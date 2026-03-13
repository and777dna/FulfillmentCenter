using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Repositories.Interfaces;

public interface IOrderRepository
{
    public void Create(Order order);
    public void Delete(Guid id);
    public Task<List<Order>> Read();
    public void UpdateOrder<TUpdateParam>(TUpdateParam updateParam, Guid orderId, Action<Order, TUpdateParam> up);
}
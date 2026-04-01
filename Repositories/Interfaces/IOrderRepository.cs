using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Repositories.Interfaces;

public interface IOrderRepository
{
    public Task Create(Order order);
    public Task Delete(Guid id);
    public Task<List<Order>> Read();
    public Task UpdateOrder<TUpdateParam>(TUpdateParam updateParam, Guid orderId, Action<Order, TUpdateParam> up);
}
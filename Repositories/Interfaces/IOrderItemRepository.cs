using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Repositories.Interfaces;

public interface IOrderItemRepository
{
    public Task Create(OrderItem orderItem);
    public Task Delete(Guid id);
    public Task<List<OrderItem>> Read();
    public Task UpdateOrderItem();
}
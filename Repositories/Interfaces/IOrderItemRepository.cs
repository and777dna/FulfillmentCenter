using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Repositories.Interfaces;

public interface IOrderItemRepository
{
    public Task CreateAsync(OrderItem orderItem);
    public Task DeleteAsync(Guid id);
    public Task<List<OrderItem>> ReadAsync();
    public Task UpdateOrderItemAsync();
}
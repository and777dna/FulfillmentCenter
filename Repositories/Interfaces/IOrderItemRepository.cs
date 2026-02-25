using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Repositories.Interfaces;

public interface IOrderItemRepository
{
    public void Create(OrderItem orderItem);
    public void Delete(Guid id);
    public List<OrderItem> Read();
    public void UpdateOrderItem();
}
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Repositories.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    public Task<List<Order>> ReadAsync(QueryParams queryParams);
    public Task UpdateOrderAsync<TUpdateParam>(TUpdateParam updateParam, Guid orderId, Action<Order, TUpdateParam> up);
    Task<Order> GetOrderWithItemsAsync(Guid orderId);
}
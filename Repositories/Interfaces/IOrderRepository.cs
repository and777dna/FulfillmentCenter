using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Repositories.Interfaces;

public interface IOrderRepository
{
    public Task CreateAsync(Order order);
    public Task DeleteAsync(Guid id);
    public Task<List<Order>> ReadAsync();
    public Task<PagedResult<ResponseOrderDto>> ReadAsync(QueryParams queryParams);
    public Task UpdateOrderAsync<TUpdateParam>(TUpdateParam updateParam, Guid orderId, Action<Order, TUpdateParam> up);
    public Task<Order> GetOrderById(Guid orderId);
}
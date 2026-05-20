using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;

namespace FulfillmentCenter.Services.Interfaces;

public interface IOrderService
{
    public Task CreateOrder(RequestOrderDto orderDto, string idempotencyKey);
    public Task CancelOrder(Guid orderId);
    public Task<Order> GetOrderById(Guid orderId);
}
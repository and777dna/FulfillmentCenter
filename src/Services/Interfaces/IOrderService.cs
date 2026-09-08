using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;

namespace FulfillmentCenter.Services.Interfaces;

public interface IOrderService
{
    public Task CreateOrder(RequestOrderDto orderDto, string idempotencyKey, RequestOrderItemDto orderItemDto);
    public Task CancelOrder(Guid orderId);
    public Task<ResponseOrderDto> GetOrderById(Guid orderId);
    public Task<PagedResult<ResponseOrderDto>> GetOrders(QueryParams queryParams);
    Task UpdateOrder(Guid orderId, RequestOrderItemDto orderItemDto, Guid centerId);
}
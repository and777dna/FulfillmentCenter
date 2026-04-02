using FulfillmentCenter.DTOs.Requests;

namespace FulfillmentCenter.Services.Interfaces;

public interface IOrderItemService
{
    public Task AddOrderItemToOrder(RequestOrderItemDto? orderItemDto);
}
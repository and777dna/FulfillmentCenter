using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Services.Interfaces;

public interface IOrder
{
    public void CreateOrder(RequestOrderDto orderDto);
    public void CancelOrder(Guid orderId);
}
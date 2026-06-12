using FulfillmentCenter.Enums;

namespace FulfillmentCenter.Services.UpdateOrderStatus.Interfaces;

public interface IOrderStatusHandler
{
    Task HandleAsync(Guid orderId);
    OrderStatus SupportedStatus { get; }
}
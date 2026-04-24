using FulfillmentCenter.Enums;

namespace FulfillmentCenter.Services.Handlers;

public interface IOrderStatusHandler
{
    Task HandleAsync(Guid orderId);
    OrderStatus SupportedStatus { get; }
}
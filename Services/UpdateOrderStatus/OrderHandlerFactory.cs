using FulfillmentCenter.Enums;
using FulfillmentCenter.Services.Handlers;

namespace FulfillmentCenter.Services.UpdateOrderStatus;

public class OrderHandlerFactory
{
    private readonly IEnumerable<IOrderStatusHandler> _handlers;

    public IOrderStatusHandler GetHandler(OrderStatus orderStatus)
    {
        return _handlers.First(handler => handler.SupportedStatus == orderStatus);
    }
}
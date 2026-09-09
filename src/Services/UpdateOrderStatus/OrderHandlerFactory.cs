using FulfillmentCenter.Enums;
using FulfillmentCenter.Services.UpdateOrderStatus.Interfaces;

namespace FulfillmentCenter.Services.UpdateOrderStatus;

public class OrderHandlerFactory(IEnumerable<IOrderStatusHandler> handlers)
{
    public IOrderStatusHandler GetHandler(OrderStatus orderStatus)
    {
        return handlers.First(handler => handler.SupportedStatus == orderStatus);
    }
}
namespace FulfillmentCenter.Exceptions;

public class OrderNotFoundException : Exception
{
    public Guid OrderId { get; }

    public OrderNotFoundException(Guid orderId)
        : base($"Order with id {orderId} was not found.")
    {
        OrderId = orderId;
    }
}

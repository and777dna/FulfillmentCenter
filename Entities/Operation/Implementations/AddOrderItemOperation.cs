using FulfillmentCenter.Entities.Operation.Interfaces;

namespace FulfillmentCenter.Entities.Operation.Implementations;

public class AddOrderItemOperation(int amount) : IOperation
{
    public void Apply(OrderItem orderItem)
    {
        orderItem.Increase(amount);
    }
}
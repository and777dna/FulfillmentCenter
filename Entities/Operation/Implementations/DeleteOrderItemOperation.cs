using FulfillmentCenter.Entities.Operation.Interfaces;

namespace FulfillmentCenter.Entities.Operation.Implementations;

public class DeleteOrderItemOperation(int amount) : IOperation
{
    public void Apply(OrderItem orderItem)
    {
        orderItem.Decrease(amount);
    }
}
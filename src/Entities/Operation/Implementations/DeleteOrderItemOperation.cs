using FulfillmentCenter.Entities.Operation.Interfaces;

namespace FulfillmentCenter.Entities.Operation.Implementations;

public class DeleteOrderItemOperation(int amount) : IOperation<OrderItem>, IOperation<Inventory>
{
    public void Apply(OrderItem orderItem)
    {
        orderItem.Decrease(amount);
    }

    public void Apply(Inventory inventory)
    {
        inventory.Increase(amount);
    }
}
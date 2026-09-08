using FulfillmentCenter.Entities.Operation.Interfaces;

namespace FulfillmentCenter.Entities.Operation.Implementations;

public class AddOrderItemOperation(int amount) : IOperation<OrderItem>, IOperation<Inventory>
{
    public void Apply(OrderItem orderItem)
    {
        orderItem.Increase(amount);
    }

    public void Apply(Inventory inventory)
    {
        inventory.Decrease(amount);
    }
}
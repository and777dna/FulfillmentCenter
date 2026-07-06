namespace FulfillmentCenter.Entities.Operation.Interfaces;

public interface IOperation
{
    public void Apply(OrderItem orderItem);
}
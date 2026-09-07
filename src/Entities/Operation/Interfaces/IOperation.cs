namespace FulfillmentCenter.Entities.Operation.Interfaces;

public interface IOperation<TEntity>
{
    public void Apply(TEntity entity);
}
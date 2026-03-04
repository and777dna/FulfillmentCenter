namespace FulfillmentCenter.Repositories.Interfaces;

public interface IFulfillmentCenterRepository
{
    public void Create(Entities.FulfillmentCenter fulfillmentCenter);
    public void Delete(Guid id);
    public List<Entities.FulfillmentCenter> Read();

    public void UpdateFulfillmentCenter<TUpdateParam>(Guid FulfillmentCenterId, TUpdateParam updateParam,
        Action<TUpdateParam, Entities.FulfillmentCenter> up);
}
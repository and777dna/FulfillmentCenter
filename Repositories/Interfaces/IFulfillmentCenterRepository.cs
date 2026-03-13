using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Repositories.Interfaces;

public interface IFulfillmentCenterRepository
{
    public void Create(Entities.DistributionCenter distributionCenter);
    public void Delete(Guid id);
    public Task<List<DistributionCenter>> Read();

    public void UpdateFulfillmentCenter<TUpdateParam>(Guid FulfillmentCenterId, TUpdateParam updateParam,
        Action<TUpdateParam, Entities.DistributionCenter> up);

    public void UpdateInventory(Guid FulfillmentCenterId, Inventory inventory);
}
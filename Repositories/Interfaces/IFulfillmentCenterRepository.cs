using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Repositories.Interfaces;

public interface IFulfillmentCenterRepository
{
    public Task Create(Entities.DistributionCenter distributionCenter);
    public Task Delete(Guid id);
    public Task<List<DistributionCenter>> Read();

    public Task UpdateFulfillmentCenter<TUpdateParam>(Guid FulfillmentCenterId, TUpdateParam updateParam,
        Action<TUpdateParam, Entities.DistributionCenter> up);
}
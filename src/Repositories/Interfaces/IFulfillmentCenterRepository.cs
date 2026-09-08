using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Repositories.Interfaces;

public interface IFulfillmentCenterRepository
{
    public Task CreateAsync(Entities.DistributionCenter distributionCenter);
    public Task DeleteAsync(Guid id);
    public Task<List<DistributionCenter>> ReadAsync();

    public Task UpdateFulfillmentCenterAsync<TUpdateParam>(Guid FulfillmentCenterId, TUpdateParam updateParam,
        Action<TUpdateParam, Entities.DistributionCenter> up);
}
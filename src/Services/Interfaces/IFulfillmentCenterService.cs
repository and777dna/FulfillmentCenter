using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Services.Interfaces;

public interface IFulfillmentCenterService
{
    public Task<DistributionCenter> FindFulfillmentCenter(Guid centerId);
}
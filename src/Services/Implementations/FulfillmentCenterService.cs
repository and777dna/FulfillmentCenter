using System.ComponentModel.DataAnnotations;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class FulfillmentCenterService : IFulfillmentCenterService
{
    private IFulfillmentCenterRepository _fulfillmentCenterRepository;
    private Lazy<Task<List<DistributionCenter>>> _distributionCenters;

    public FulfillmentCenterService(IFulfillmentCenterRepository fulfillmentCenterRepository)
    {
        _fulfillmentCenterRepository = fulfillmentCenterRepository;
        ResetCache();
    }

    private void ResetCache()
    {
        _distributionCenters = new Lazy<Task<List<DistributionCenter>>>(() => _fulfillmentCenterRepository.ReadAsync());
    }
    
    public async Task<DistributionCenter> FindFulfillmentCenter(Guid centerId)
    {
        var centers = await _distributionCenters.Value;
        
        var fulfillmentCenter = centers.FirstOrDefault(center => center.Id == centerId);
        if (fulfillmentCenter != null)
        {
            return fulfillmentCenter;
        }

        throw new ValidationException();
    }
}
using System.ComponentModel.DataAnnotations;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class FulfillmentCenterService(IFulfillmentCenterRepository fulfillmentCenterRepository) : IFulfillmentCenterService
{
    private IFulfillmentCenterRepository _fulfillmentCenterRepository = fulfillmentCenterRepository;
    public async Task<DistributionCenter> FindFulfillmentCenter(Guid centerId)
    {
        var fulfillmentCenters = await _fulfillmentCenterRepository.Read();
        var fulfillmentCenter = fulfillmentCenters.FirstOrDefault(center => center.Id == centerId);
        if (fulfillmentCenter != null)
        {
            return fulfillmentCenter;
        }

        throw new ValidationException();
    }
}
using System.ComponentModel.DataAnnotations;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class FulfillmentCenterService(IFulfillmentCenterRepository fulfillmentCenterRepository) : IFulfillmentCenterService
{
    private IFulfillmentCenterRepository _fulfillmentCenterRepository = fulfillmentCenterRepository;
    public async Task<DistributionCenter> FindFulfillmentCenter(Guid centerId)
    {// 0f8fad5b-d9cb-469f-a165-70867728950e
        var fulfillmentCenters = await _fulfillmentCenterRepository.Read();
        var fulfillmentCenter = fulfillmentCenters.FirstOrDefault(center => center.Id == centerId);
        if (fulfillmentCenter != null)
        {
            return fulfillmentCenter;
        }

        throw new ValidationException();
    }
}
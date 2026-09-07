using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Strategies.Interfaces;

namespace FulfillmentCenter.Strategies.Implementations;

public class FirstAvailableCenterStrategy : IShipmentAssignmentStrategy
{
    private IFulfillmentCenterRepository _fulfillmentCenterRepository;
    private List<DistributionCenter>? _cache;

    public FirstAvailableCenterStrategy(IFulfillmentCenterRepository fulfillmentCenterRepository)
    {
        _fulfillmentCenterRepository = fulfillmentCenterRepository;
    }
    public async Task<Guid> SelectDistributionCenter(Guid productId, int quantity)
    {
        var _distributionCenters = _cache ??= await _fulfillmentCenterRepository.ReadAsync();
        var distributionCenter = _distributionCenters
            .FirstOrDefault(center =>
                center.Inventories
                    .Where(i => i.Product.Id == productId)
                    .Sum(i => i.Quantity) >= quantity);
        
        return distributionCenter.Id;
    }
}
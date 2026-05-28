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
        // _distributionCenters = new Lazy<Task<List<DistributionCenter>>>(() => _fulfillmentCenterRepository.Read());
    }
    public async Task<Guid> SelectDistributionCenter(Guid productId, int quantity)
    {
        var _distributionCenters = _cache ??= await _fulfillmentCenterRepository.Read();
        var distributionCenter = _distributionCenters
            .FirstOrDefault(center =>
                center.Inventories
                    .Where(i => i.Product.Id == productId)
                    .Sum(i => i.Quantity) >= quantity);
        
        return distributionCenter.Id;
    }
}
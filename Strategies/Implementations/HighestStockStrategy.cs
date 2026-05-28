using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Strategies.Interfaces;

namespace FulfillmentCenter.Strategies.Implementations;
//HighestStockStrategy - берёт центр с наибольшим остатком нужного товара
public class HighestStockStrategy : IShipmentAssignmentStrategy
{
    private IFulfillmentCenterRepository _fulfillmentCenterRepository;
    private List<DistributionCenter>? _cache;

    public HighestStockStrategy(IFulfillmentCenterRepository fulfillmentCenterRepository)
    {
        _fulfillmentCenterRepository = fulfillmentCenterRepository;
    }
    public async Task<Guid> SelectDistributionCenter(Guid productId, int quantity)
    {
        var distributionCenters = _cache ??= await _fulfillmentCenterRepository.ReadAsync();
        //distributionCenters.First(distributionCenter => distributionCenter.);
        var distributionCenter = distributionCenters
            .Select(center => new
            {
                Center = center,
                Stock = center.Inventories
                    .Where(i => i.Product.Id == productId)
                    .Sum(i => i.Quantity)
            })
            .Where(x => x.Stock >= quantity)
            .OrderByDescending(x => x.Stock)
            .FirstOrDefault()
            ?.Center;
        return distributionCenter.Id;
    }
}
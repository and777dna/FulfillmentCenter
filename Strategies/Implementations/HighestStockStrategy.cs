using FulfillmentCenter.Entities;
using FulfillmentCenter.Strategies.Interfaces;

namespace FulfillmentCenter.Strategies.Implementations;
//HighestStockStrategy - берёт центр с наибольшим остатком нужного товара
public class HighestStockStrategy : IShipmentAssignmentStrategy
{
    public DistributionCenter SelectDistributionCenter(Guid productId, int quantity, IReadOnlyCollection<DistributionCenter> distributionCenters)
    {
        //distributionCenters.First(distributionCenter => distributionCenter.);
        return distributionCenters
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
    }
}
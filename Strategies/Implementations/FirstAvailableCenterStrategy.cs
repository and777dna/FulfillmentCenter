using FulfillmentCenter.Entities;
using FulfillmentCenter.Strategies.Interfaces;

namespace FulfillmentCenter.Strategies.Implementations;

public class FirstAvailableCenterStrategy : IShipmentAssignmentStrategy
{
    public DistributionCenter SelectDistributionCenter(Guid productId, int quantity, IReadOnlyCollection<DistributionCenter> distributionCenters)
    {
        return distributionCenters
            .FirstOrDefault(center =>
                center.Inventories
                    .Where(i => i.Product.Id == productId)
                    .Sum(i => i.Quantity) >= quantity);
    }
}
using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Strategies.Interfaces;

public interface IShipmentAssignmentStrategy
{//HighestStockStrategy - берёт центр с наибольшим остатком нужного товара
    //FirstAvailableCenterStrategy - берёт первый центр с нужным товаром в наличии
    public DistributionCenter SelectDistributionCenter(
        Guid productId,
        int quantity,
        IReadOnlyCollection<DistributionCenter> distributionCenters);
}
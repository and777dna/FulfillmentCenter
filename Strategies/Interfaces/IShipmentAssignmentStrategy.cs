using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Strategies.Interfaces;

public interface IShipmentAssignmentStrategy
{ 
        public Task<Guid> SelectDistributionCenter(
        Guid productId,
        int quantity);
}
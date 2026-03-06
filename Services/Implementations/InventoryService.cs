using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Implementations;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class InventoryService(IInventoryRepository inventoryRepository, IFulfillmentCenterRepository fulfillmentCenterRepository) : IInventoryService
{
    private IInventoryRepository _inventoryRepository = inventoryRepository;
    private IFulfillmentCenterRepository _fulfillmentCenterRepository = fulfillmentCenterRepository;
    
    public void AddStock(Inventory inventory, Guid fulfillmentCenterId)
    {
        _fulfillmentCenterRepository.UpdateInventory(fulfillmentCenterId, inventory);
    }
    
    public ICollection<Inventory> RemainingsOnTheFulfillmentCenter(Guid centerId)
    { 
        var fulfillmentCenters = _fulfillmentCenterRepository.Read();
        var findCenter = fulfillmentCenters.FirstOrDefault(center => center.Id == centerId);
        return findCenter.Inventory;
    }
}
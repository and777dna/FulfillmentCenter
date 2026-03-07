using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Implementations;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class InventoryService(IInventoryRepository inventoryRepository, IFulfillmentCenterRepository fulfillmentCenterRepositor) : IInventoryService
{
    private IInventoryRepository _inventoryRepository = inventoryRepository;
    private IFulfillmentCenterRepository _fulfillmentCenterRepositor = fulfillmentCenterRepositor;
    
    public void AddStock(RequestInventoryDto inventoryDto, Guid fulfillmentCenterId)//пополнить остатки
    {
        Inventory inventory = new Inventory
        {
            Id = Guid.NewGuid(),
            ProductId = inventoryDto.ProductId,
            Quantity = inventoryDto.Quantity,
            Product = inventoryDto.Product,
            DistributionCenterId = fulfillmentCenterId,
            DistributionCenter = inventoryDto.DistributionCenter
        };
        //на конкретном складе
        _fulfillmentCenterRepositor.UpdateInventory(fulfillmentCenterId, inventory);
    }
    
    ////GET	/api/inventory/{centerId}	Остатки на складе
    public ICollection<Inventory> RemainingsOnTheFulfillmentCenter(Guid centerId)
    { 
        var fulfillmentCenters = _fulfillmentCenterRepositor.Read();
        var findedCenter = fulfillmentCenters.FirstOrDefault(center => center.Id == centerId);
        return findedCenter.Inventory;
    }
}
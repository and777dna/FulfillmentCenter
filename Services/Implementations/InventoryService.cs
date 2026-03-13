using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Implementations;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;
using Microsoft.EntityFrameworkCore.ValueGeneration;

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
    public async Task<ICollection<Inventory>> RemainingsOnTheFulfillmentCenter(Guid centerId)
    { 
        var fulfillmentCenters = await _fulfillmentCenterRepositor.Read();
        var findCenter = fulfillmentCenters.FirstOrDefault(center => center.Id == centerId);
        return findCenter.Inventory;
    }

    public Dictionary<Guid, int> ReturnProductAmount(ICollection<Inventory> inventories)
    {
        Dictionary<Guid, int> openWith = new Dictionary<Guid, int>();
        foreach (var product in inventories)
        {
            openWith.Add(product.Id, product.Quantity);
        }

        return openWith;
    }
    
    /*public bool CheckSufficientAmountOfInventory(ICollection<Inventory> remainingsOnTheFulfillmentCenter, ICollection<OrderItem> items)
    {
        //TODO: to create for each remainingsOnTheFulfillmentCenter,items hash to compare them.
        foreach (var inventory in remainingsOnTheFulfillmentCenter)
        {
            
        }

        return true;
    }*/
}
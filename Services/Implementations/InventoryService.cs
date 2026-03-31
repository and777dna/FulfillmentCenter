using System.ComponentModel.DataAnnotations;
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class InventoryService(IInventoryRepository inventoryRepository, IFulfillmentCenterRepository fulfillmentCenterRepositor, IFulfillmentCenterService fulfillmentCenterService, IProductService productService) : IInventoryService
{
    private IInventoryRepository _inventoryRepository = inventoryRepository;
   
    
    private IFulfillmentCenterRepository _fulfillmentCenterRepositor = fulfillmentCenterRepositor;
    private IFulfillmentCenterService _fulfillmentCenterService = fulfillmentCenterService;
    private IProductService _productService = productService;
    
    public async Task AddStock(RequestInventoryDto inventoryDto, Guid fulfillmentCenterId)//пополнить остатки
    {
        //TODO: if "fulfillmentCenterId" exist -> should be BOOL THIS ONE to delete?????
        //var fulfillmentCenter = await FindProduct(fulfillmentCenterId, inventoryDto.ProductId);
        //TODO: if "productId" exist -> should be BOOL 
        var productOnFulfillmentCenter = await FindProduct(fulfillmentCenterId, inventoryDto.ProductId);//TODO: to add then number of products if exists
        
        
        Inventory inventory = new Inventory
        {
            Id = Guid.NewGuid(),
            ProductId = inventoryDto.ProductId,
            Quantity = inventoryDto.Quantity,//to add +1 or to create with label 1
            DistributionCenterId = fulfillmentCenterId,
        };
        //if(fulfillmentCenterId == true && product == true){to update inventory}
        if (productOnFulfillmentCenter != null)
        {
            await _inventoryRepository.UpdateInventory(inventory);
        }
        try
        {
            await _inventoryRepository.Create(inventory);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        //if(product == false){to create inventory}
        //на конкретном складе//TODO: the update should be inside Inventory entity
    }
    /*Id CHAR(36) PRIMARY KEY NOT NULL,
       ProductId CHAR(36),
       DistributionCenterId CHAR(36),
       Quantity INT,*/

    private async Task<bool> FindProduct(Guid fulfillmentCenterId,Guid productId)
    {
        var inventories = await _inventoryRepository.Read();
        bool productWasFound = true;
        inventories.Find(inventory =>
        {
            return productWasFound = inventory.DistributionCenterId == fulfillmentCenterId && inventory.ProductId == productId;
        });
        return productWasFound;
    }
    
    ////GET	/api/inventory/{centerId}	Остатки на складе
    public async Task<ICollection<Inventory>> RemainingsOnTheFulfillmentCenter(Guid centerId)
    { 
        var inventories = await _inventoryRepository.Read();
        var findInventoriesFromCenter = inventories.FindAll(inventory => inventory.DistributionCenterId == centerId);
        //var findCenter = fulfillmentCenters.FirstOrDefault(center => center.Id == centerId);
        if (findInventoriesFromCenter != null)
        {
            return findInventoriesFromCenter;
        }

        throw new ValidationException();
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
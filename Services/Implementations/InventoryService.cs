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
    
    public async void AddStock(RequestInventoryDto inventoryDto, Guid fulfillmentCenterId)//пополнить остатки
    {
        var distributionCenter = await _fulfillmentCenterService.FindFulfillmentCenter(fulfillmentCenterId);
        var product = await _productService.FindProduct(inventoryDto.ProductId);
        
        Inventory inventory = new Inventory
        {
            Id = Guid.NewGuid(),
            ProductId = inventoryDto.ProductId,
            Quantity = inventoryDto.Quantity,
            Product = product,
            DistributionCenterId = fulfillmentCenterId,
            DistributionCenter = distributionCenter
        };
        //на конкретном складе//TODO: the update should be inside Inventory entity
        _fulfillmentCenterRepositor.UpdateInventory(fulfillmentCenterId, inventory);
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
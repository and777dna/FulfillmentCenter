using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Implementations;
using FulfillmentCenter.Services.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class InventoryService(SqlInventoryRepository sqlInventoryRepository, SqlFulfillmentCenterRepository sqlFulfillmentCenterRepositor) : IInventory
{
    private SqlInventoryRepository _sqlInventoryRepository = sqlInventoryRepository;
    private SqlFulfillmentCenterRepository _sqlFulfillmentCenterRepository = sqlFulfillmentCenterRepositor;
    
    public void AddStock(RequestInventoryDto inventoryDto, Guid fulfillmentCenterId)//пополнить остатки
    {
        Inventory inventory = new Inventory
        {
            Id = Guid.NewGuid(),
            ProductId = inventoryDto.ProductId,
            Quantity = inventoryDto.Quantity,
            Product = inventoryDto.Product,
            DistributionCenterId = fulfillmentCenterId,
            FulfillmentCenter = inventoryDto.FulfillmentCenter
            
        };
        //на конкретном складе
        _sqlFulfillmentCenterRepository.UpdateInventory(fulfillmentCenterId, inventory);
    }
    
    ////GET	/api/inventory/{centerId}	Остатки на складе
    public ICollection<Inventory> RemainingsOnTheFulfillmentCenter(Guid centerId)
    { 
        var fulfillmentCenters = _sqlFulfillmentCenterRepository.Read();
        var findedCenter = fulfillmentCenters.FirstOrDefault(center => center.Id == centerId);
        return findedCenter.Inventory;
    }
}
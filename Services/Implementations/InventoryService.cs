using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Implementations;
using FulfillmentCenter.Services.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class InventoryService(SqlInventoryRepository sqlInventoryRepository, SqlFulfillmentCenterRepository sqlFulfillmentCenterRepositor) : IInventory
{
    private SqlInventoryRepository _sqlInventoryRepository = sqlInventoryRepository;
    private SqlFulfillmentCenterRepository _sqlFulfillmentCenterRepository = sqlFulfillmentCenterRepositor;
    
    public void AddStock(Inventory inventory, Guid fulfillmentCenterId)//пополнить остатки
    {
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
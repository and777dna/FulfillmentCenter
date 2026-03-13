using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Services.Interfaces;

public interface IInventoryService
{
    public void AddStock(RequestInventoryDto inventoryDto, Guid fulfillmentCenterId);
    public ICollection<Inventory> RemainingsOnTheFulfillmentCenter(Guid centerId);

    public Dictionary<Guid, int> ReturnProductAmount(ICollection<Inventory> inventories);
}
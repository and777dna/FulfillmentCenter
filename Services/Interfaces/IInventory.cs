using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Services.Interfaces;

public interface IInventory
{
    public void AddStock(Inventory inventory, Guid fulfillmentCenterId);
    public ICollection<Inventory> RemainingsOnTheFulfillmentCenter(Guid centerId);
}
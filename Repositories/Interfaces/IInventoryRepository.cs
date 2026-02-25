using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Repositories.Interfaces;

public interface IInventoryRepository
{
    public void Create(Inventory inventory);
    public void Delete(Guid id);
    public List<Inventory> Read();
    public void UpdateInventory();
}
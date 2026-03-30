using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Repositories.Interfaces;

public interface IInventoryRepository
{
    public Task Create(Inventory inventory);
    public void Delete(Guid id);
    public Task<List<Inventory>> Read();
    public Task UpdateInventory(Inventory inventory);
}
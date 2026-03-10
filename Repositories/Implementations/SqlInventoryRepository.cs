using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlInventoryRepository : IInventoryRepository
{
    private FulfillmentCenDbContext _context;
    public List<Inventory> Inventories;
    private bool isCached;
    
    public SqlInventoryRepository(FulfillmentCenDbContext context)
    {
        _context = context;
        Inventories = Read();
        isCached = true;
    }

    public void Create(Inventory inventory)
    {
        _context.Inventories.Add(inventory);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var inventoryToDelete = _context.Inventories.FirstOrDefault(inventory => inventory.Id == id);
        _context.Inventories.Remove(inventoryToDelete);
        _context.SaveChanges();
    }

    public List<Inventory> Read()
    {
        if (isCached == false)
        {
            List<Inventory> inventories = _context.Inventories.ToList();
            isCached = true;
            return inventories;
        }

        return Inventories;
    }
    public void UpdateInventory(){}
}
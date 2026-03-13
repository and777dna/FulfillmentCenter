using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlInventoryRepository : IInventoryRepository
{
    private FulfillmentCenDbContext _context;
    public List<Inventory> Inventories;
    private bool isCached;
    
    public SqlInventoryRepository(FulfillmentCenDbContext context)
    {
        _context = context;
        Inventories = Read().Result;
        isCached = true;
    }

    public async void Create(Inventory inventory)
    {
        await _context.Inventories.AddAsync(inventory);
        await _context.SaveChangesAsync();
    }

    public async void Delete(Guid id)
    {
        var inventoryToDelete = await _context.Inventories.FirstOrDefaultAsync(inventory => inventory.Id == id);
        _context.Inventories.Remove(inventoryToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Inventory>> Read()
    {
        if (isCached == false)
        {
            List<Inventory> inventories = await _context.Inventories.ToListAsync();
            isCached = true;
            return inventories;
        }

        return Inventories;
    }
    public void UpdateInventory(){}
}
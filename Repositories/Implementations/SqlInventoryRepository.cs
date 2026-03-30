using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlInventoryRepository : IInventoryRepository
{
    private FulfillmentCenDbContext _context;
    //public List<Inventory> Inventories;
    //private bool isCached;
    
    public SqlInventoryRepository(FulfillmentCenDbContext context)
    {
        _context = context;
        /*Inventories = Read().Result;
        isCached = true;*/
    }

    public async Task Create(Inventory inventory)
    {
        try
        {
            await _context.Inventory.AddAsync(inventory);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async void Delete(Guid id)
    {
        var inventoryToDelete = await _context.Inventory.FirstOrDefaultAsync(inventory => inventory.Id == id);
        _context.Inventory.Remove(inventoryToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Inventory>> Read()
    {
        //if (isCached == false)
        //{
            List<Inventory> inventories = await _context.Inventory.ToListAsync();
            //isCached = true;
            return inventories;
        //}

        //return Inventories;
    }
    public async Task UpdateInventory(Inventory inventory)
    {
        /*UpdateInventoryQuantity(FulfillmentCenterId, inventory,
            (inventory, fulfillmentCente) =>
            {
                var InventoryToUpdate = fulfillmentCente.Inventory.FirstOrDefault(inventor => inventor.Id == inventory.Id);
                InventoryToUpdate = inventory; 
            });*/
    }

    public async Task UpdateInventoryQuantity()
    {
        
    }
}
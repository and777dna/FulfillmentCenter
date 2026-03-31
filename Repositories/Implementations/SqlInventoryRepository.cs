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
        List<Inventory> inventories;
        //if (isCached == false)
        //{
        try
        {
            inventories = await _context.Inventory.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
            //isCached = true;
            return inventories;
        //}

        //return Inventories;
    }
    public async Task UpdateInventory(Inventory inventory)
    {
        try
        {
            var inventoryToUpdate = await _context.Inventory.FirstOrDefaultAsync(inv =>
                inv.ProductId == inventory.ProductId && inv.DistributionCenterId == inventory.DistributionCenterId);
            inventoryToUpdate.Quantity += 1;

            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        /*UpdateInventoryQuantity(fulfillmentCenterId, inventory,
            (inventory, fulfillmentCente) =>
            {
                var InventoryToUpdate = _context.Inventory.FirstOrDefault(inventor => { return inventor.Id == inventory.Id && inventor.DistributionCenter == inventory. } );
                InventoryToUpdate = inventory;
            });*/
    }

    public async Task UpdateInventoryQuantity(Inventory inventory)
    {
        _context.SaveChangesAsync();
    }
}
using FulfillmentCenter.Data;
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlInventoryRepository : IInventoryRepository
{
    private readonly FulfillmentCenDbContext _context;
    
    public SqlInventoryRepository(FulfillmentCenDbContext context)
    {
        _context = context;
    }

    public async Task Create(Inventory inventory)
    {
        try
        {
            await _context.Inventories.AddAsync(inventory);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task Delete(Guid id)
    {
        var inventoryToDelete = await _context.Inventories.FirstOrDefaultAsync(inventory => inventory.Id == id);
        if(inventoryToDelete == null)
        {
            throw new ArgumentNullException(nameof(id), "no Inventory was found");
        }
        inventoryToDelete.IsDeleted = true;
        await _context.SaveChangesAsync();
    }

    public async Task<List<Inventory>> Read()
    {//All Read() methods load the entire table into memory as a List<T>. No filtering, no Where, no pagination. This will not scale
        List<Inventory> inventories;
            try
            {
                inventories = await _context.Inventories.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return inventories;
    }
    
    public async Task UpdateInventory(Inventory inventory)
    {
        try
        {
            var inventoryToUpdate = await _context.Inventories.FirstOrDefaultAsync(inv =>
                inv.ProductId == inventory.ProductId && inv.DistributionCenterId == inventory.DistributionCenterId);

            if (inventoryToUpdate == null)
            {
                throw new ArgumentNullException(nameof(inventory.ProductId), "Inventory was not found");
            }
            inventoryToUpdate.Quantity += inventory.Quantity;

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

    public async Task UpdateInventoryQuantity(UpdateInventoryDto inventory)
    {
        try
        {
            var inventoryToUpdate = await _context.Inventories.FirstOrDefaultAsync(inv =>
                inv.ProductId == inventory.ProductId);

            if (inventoryToUpdate == null)
            {
                throw new ArgumentNullException(nameof(inventoryToUpdate.ProductId), "InventoryToUpdate was not found");
            }
            
            inventoryToUpdate.Quantity += inventory.Quantity;

            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
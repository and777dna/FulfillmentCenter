using FulfillmentCenter.Data;
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlInventoryRepository : IInventoryRepository
{
    private FulfillmentCenDbContext _context;
    public List<Inventory> Inventories;
    private bool _isCached;
    
    public SqlInventoryRepository(FulfillmentCenDbContext context)
    {
        _context = context;
        Inventories = Read().Result;
        _isCached = true;
    }

    public async Task Create(Inventory inventory)
    {
        try
        {
            await _context.Inventory.AddAsync(inventory);
            await _context.SaveChangesAsync();
            _isCached = false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task Delete(Guid id)
    {
        var inventoryToDelete = await _context.Inventory.FirstOrDefaultAsync(inventory => inventory.Id == id);
        if(inventoryToDelete == null)
        {
            throw new ArgumentNullException();
        }
        _context.Inventory.Remove(inventoryToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Inventory>> Read()
    {//All Read() methods load the entire table into memory as a List<T>. No filtering, no Where, no pagination. This will not scale
        List<Inventory> inventories;
        if (_isCached == false)
        {
            try
            {
                inventories = await _context.Inventory.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            _isCached = true;
            return inventories;
        }

        return Inventories;
    }
    
    public async Task UpdateInventory(Inventory inventory)
    {
        try
        {
            var inventoryToUpdate = await _context.Inventory.FirstOrDefaultAsync(inv =>
                inv.ProductId == inventory.ProductId && inv.DistributionCenterId == inventory.DistributionCenterId);

            if (inventoryToUpdate == null)
            {
                throw new ArgumentNullException();
            }
            inventoryToUpdate.Quantity = inventory.Quantity;

            await _context.SaveChangesAsync();
            _isCached = false;
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
            var inventoryToUpdate = await _context.Inventory.FirstOrDefaultAsync(inv =>
                inv.ProductId == inventory.ProductId);

            if (inventoryToUpdate == null)
            {
                throw new ArgumentNullException();
            }
            
            inventoryToUpdate.Quantity = inventory.Quantity;

            await _context.SaveChangesAsync();
            _isCached = false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
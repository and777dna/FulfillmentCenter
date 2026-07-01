using FulfillmentCenter.Data;
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlInventoryRepository(FulfillmentCenterDbContext context, ILogger<SqlInventoryRepository> logger) : IInventoryRepository
{
    public async Task CreateAsync(Inventory inventory)
    {
        try
        {
            await context.Inventories.AddAsync(inventory);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "not possible to create an inventory.");
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var inventoryToDelete = await context.Inventories.FirstOrDefaultAsync(inventory => inventory.Id == id);
        if(inventoryToDelete == null)
        {
            throw new ArgumentNullException(nameof(id), "no Inventory was found");
        }
        inventoryToDelete.IsDeleted = true;
        await context.SaveChangesAsync();
    }

    public async Task<List<Inventory>> ReadAsync()
    {
        List<Inventory> inventories;
            try
            {
                inventories = await context.Inventories
                    .OrderBy(p => p.Id)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, "not possible to read products.");
                throw;
            }
            return inventories;
    }
    
    public async Task<List<Inventory>> ReadAsync(QueryParams queryParams)
    {
        int page = queryParams.Page;
        int pageSize = queryParams.PageSize;
        
        List<Inventory> inventories;
        try
        {
            inventories = await context.Inventories
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToListAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "not possible to read products.");
            throw;
        }
        return inventories;
    }
    
    public async Task UpdateInventoryAsync(Inventory inventory)
    {
        try
        {
            var inventoryToUpdate = await context.Inventories.FirstOrDefaultAsync(inv =>
                inv.ProductId == inventory.ProductId && inv.DistributionCenterId == inventory.DistributionCenterId);

            if (inventoryToUpdate == null)
            {
                throw new ArgumentNullException(nameof(inventory.ProductId), "Inventory was not found");
            }
            inventoryToUpdate.Quantity += inventory.Quantity;

            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "not possible to update an inventory.");
            throw;
        }
    }

    public async Task UpdateInventoryQuantityAsync(UpdateInventoryDto inventory)
    {
        try
        {
            var inventoryToUpdate = await context.Inventories.FirstOrDefaultAsync(inv =>
                inv.ProductId == inventory.ProductId);

            if (inventoryToUpdate == null)
            {
                throw new ArgumentNullException(nameof(inventoryToUpdate.ProductId), "InventoryToUpdate was not found");
            }
            
            inventoryToUpdate.Quantity += inventory.Quantity;

            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "not possible to update an inventory quantity.");
            throw;
        }
    }
}
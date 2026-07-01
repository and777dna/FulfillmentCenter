using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlRepository<T>(FulfillmentCenterDbContext context, ILogger<SqlRepository<T>> logger) : IRepository<T> where T : BaseEntity
{
    public async Task<T?> GetByIdAsync(Guid id)
    {
        var findByIdAsync = await context.Set<T>().FindAsync(id);
        if (findByIdAsync != null) return findByIdAsync;
        logger.LogWarning("no item found: " + $"{typeof(T)}");
        throw new KeyNotFoundException();
    }

    public async Task<List<T>> GetAllAsync()
    {
        var items = await context.Set<T>().ToListAsync();
        if(items.Count == 0){
            logger.LogWarning("no items found: " + $"{typeof(T)}");
        }
        return items;
    }

    public async Task AddAsync(T? entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        try
        {
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        }
        catch (Exception)
        {
            logger.LogError("not possible to create an item");
            throw;
        }
    }

    public Task Update(T entity)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid id)
    {
        var itemToDelete = await GetByIdAsync(id);
        itemToDelete!.IsDeleted = true;
    }
}
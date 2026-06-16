using FulfillmentCenter.Data;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlRepository<T>(FulfillmentCenDbContext context, ILogger logger) : IRepository<T> where T : class
{
    public async Task<T?> GetByIdAsync(Guid id)
    {
        var findByIdAsync = await context.Set<T>().FindAsync(id);
        if (findByIdAsync == null)
        {
            logger.LogWarning("no item found: " + $"{typeof(T)}");
            throw new KeyNotFoundException();
        }
        return findByIdAsync;
    }

    public async Task<List<T>> GetAllAsync()
    {
        var items = await context.Set<T>().ToListAsync();
        if(items.Count == 0){
            logger.LogWarning("no items found: " + $"{typeof(T)}");
        }
        return items;
    }

    public Task Add(T entity)
    {
        throw new NotImplementedException();
    }

    public Task Update(T entity)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}
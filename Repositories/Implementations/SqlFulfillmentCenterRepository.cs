using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlFulfillmentCenterRepository : IFulfillmentCenterRepository
{
    private readonly FulfillmentCenDbContext _context;
    int page = 1;
    int pageSize = 50;
    public SqlFulfillmentCenterRepository(FulfillmentCenDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(DistributionCenter distributionCenter)
    {
        await _context.DistributionCenters.AddAsync(distributionCenter);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var fulfillmentCenterToDelete = await _context.DistributionCenters.FirstOrDefaultAsync(center => center.Id == id);
        if(fulfillmentCenterToDelete == null)
        {
            throw new ArgumentNullException(nameof(id), "no FulfillmentCenter was found");
        }
        fulfillmentCenterToDelete.IsDeleted = true;
        await _context.SaveChangesAsync();
    }

    public async Task<List<DistributionCenter>> ReadAsync()
    {//All Read() methods load the entire table into memory as a List<T>. No filtering, no Where, no pagination. This will not scale
        List<DistributionCenter> fulfillmentCenters = await _context.DistributionCenters.Skip((page - 1) * pageSize)
            .Include(d => d.Inventories)//TODO: to take this into account, i didnt get thgotuh navigation properties "Inventories"
            //THIS INCLUDE TO GET RID OF N+1 PROBLEM?
            .Take(pageSize).OrderBy(p => p.Id)
            .ToListAsync();
        return fulfillmentCenters;
    }

    public async Task UpdateFulfillmentCenterAsync<TUpdateParam>(Guid FulfillmentCenterId, TUpdateParam updateParam, Action<TUpdateParam, Entities.DistributionCenter> up)
    {
        var fulfillmentCenterToUpdate = await _context.DistributionCenters.FirstOrDefaultAsync(center => center.Id == FulfillmentCenterId);
        if(fulfillmentCenterToUpdate == null)throw new KeyNotFoundException("fulfillmentCenterToUpdate wasnt found");
        up(updateParam, fulfillmentCenterToUpdate);
        await _context.SaveChangesAsync();
    }
}
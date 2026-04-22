using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

//TODO review: Across all implementations:
//3. All Read() methods load the entire table into memory as a List<T>. No filtering, no Where, no pagination. This will not scale
public class SqlFulfillmentCenterRepository : IFulfillmentCenterRepository
{
    private readonly FulfillmentCenDbContext _context;
    public SqlFulfillmentCenterRepository(FulfillmentCenDbContext context)
    {
        _context = context;
    }

    public async Task Create(DistributionCenter distributionCenter)
    {
        await _context.DistributionCenters.AddAsync(distributionCenter);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var fulfillmentCenterToDelete = await _context.DistributionCenters.FirstOrDefaultAsync(center => center.Id == id);
        if(fulfillmentCenterToDelete == null)
        {
            throw new ArgumentNullException(nameof(id), "no FulfillmentCenter was found");
        }
        fulfillmentCenterToDelete.IsDeleted = true;
        await _context.SaveChangesAsync();
    }

    public async Task<List<DistributionCenter>> Read()
    {//All Read() methods load the entire table into memory as a List<T>. No filtering, no Where, no pagination. This will not scale
        List<DistributionCenter> fulfillmentCenters = await _context.DistributionCenters.ToListAsync();
        return fulfillmentCenters;
    }

    public async Task UpdateFulfillmentCenter<TUpdateParam>(Guid FulfillmentCenterId, TUpdateParam updateParam, Action<TUpdateParam, Entities.DistributionCenter> up)
    {
        var fulfillmentCenterToUpdate = await _context.DistributionCenters.FirstOrDefaultAsync(center => center.Id == FulfillmentCenterId);
        if(fulfillmentCenterToUpdate == null)throw new KeyNotFoundException("fulfillmentCenterToUpdate wasnt found");
        up(updateParam, fulfillmentCenterToUpdate);
        await _context.SaveChangesAsync();
    }
}
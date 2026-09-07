using FulfillmentCenter.Data;
using FulfillmentCenter.Repositories.Interfaces;

namespace FulfillmentCenter.Repositories.Implementations;

public class UnitOfWork(FulfillmentCenterDbContext context, ILogger<SqlOrderRepository> logger) : IUnitOfWork
{
    public async Task SaveTransactionAsync()
    {
        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError("not possible to COMMIT a transaction");
            throw;
        }
        logger.LogInformation("transaction has been COMMITed");
    }
}
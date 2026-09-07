namespace FulfillmentCenter.Repositories.Interfaces;

public interface IUnitOfWork
{
    public Task SaveTransactionAsync();
}
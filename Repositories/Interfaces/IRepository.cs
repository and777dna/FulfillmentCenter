namespace FulfillmentCenter.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<List<T>> GetAllAsync();
    Task Add(T entity);
    Task Update(T entity);
    Task Delete(Guid id);
}
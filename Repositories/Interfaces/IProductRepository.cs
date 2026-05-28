using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Repositories.Interfaces;

public interface IProductRepository
{
    public Task CreateAsync(Product product);
    public Task DeleteAsync(Guid id);
    public Task<List<Product>> ReadAsync(int page,int pageSize);
    public Task<List<Product>> ReadAsync();
}
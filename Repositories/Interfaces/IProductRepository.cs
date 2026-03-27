using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Repositories.Interfaces;

public interface IProductRepository
{
    public Task Create(Product product);
    public void Delete(Guid id);
    public Task<List<Product>> Read();
    public void UpdateProduct();
}
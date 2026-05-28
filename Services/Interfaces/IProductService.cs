using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Services.Interfaces;

public interface IProductService
{
    public Task<List<Product>> GetProducts(int page, int pageSize);
    public Task CreateProduct(RequestProductDto product);
    public Task<Product> FindProduct(Guid productId);
}
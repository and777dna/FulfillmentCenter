using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Services.Interfaces;

public interface IProductService
{
    public Task<List<Product>> GetProducts();
    public void CreateProduct(RequestProductDto product);
}
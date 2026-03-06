using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Services.Interfaces;

public interface IProductService
{
    public List<Product> GetProducts();
    public void CreateProduct(Product product);
}
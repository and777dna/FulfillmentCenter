using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Services.Interfaces;

public interface IProduct
{
    public List<Product> GetProducts();
    public void CreateProduct(Product product);
}
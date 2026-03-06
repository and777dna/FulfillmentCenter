using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Implementations;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class ProductService(IProductRepository productRepository) : IProductService
{
    private IProductRepository _productRepository = productRepository;
    
    
    public List<Product> GetProducts()
    {
        List<Product> products = _productRepository.Read();
        return products;
    }

    public void CreateProduct(Product product)
    {
        _productRepository.Create(product);
    }
}
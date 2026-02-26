using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Implementations;
using FulfillmentCenter.Services.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class ProductService : IProduct
{
    private SqlProductRepository _sqlProductRepository;

    public ProductService(SqlProductRepository sqlProductRepository)
    {
        _sqlProductRepository = sqlProductRepository;
    }
    
    public List<Product> GetProducts()
    {
        List<Product> products = _sqlProductRepository.Read();
        return products;
    }

    public void CreateProduct(Product product)
    {
        _sqlProductRepository.Create(product);
    }
}
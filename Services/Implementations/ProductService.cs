using FulfillmentCenter.DTOs.Requests;
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

    public void CreateProduct(RequestProductDto productDto)
    {
        Product product = new Product
        {
            Id = Guid.NewGuid(),
            Name = productDto.Name,
            SKU = productDto.SKU,
            Weight = productDto.Weight
        };
        _sqlProductRepository.Create(product);
    }
}
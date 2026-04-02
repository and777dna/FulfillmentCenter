using System.ComponentModel.DataAnnotations;
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class ProductService(IProductRepository productRepository) : IProductService
{
    private IProductRepository _productRepository = productRepository;


    public async Task<List<Product>> GetProducts()
    {
        List<Product> products = await _productRepository.Read();
        return products;
    }

    public async Task CreateProduct(RequestProductDto productDto)
    {
        var productAlreadyExist = await CheckProductExist(productDto.SKU);
        if (productAlreadyExist)
        {
            throw new InvalidOperationException("Запись с таким SKU уже существует в базе данных.");
        }
        Product product = new Product
        {
            Id = Guid.NewGuid(),
            Name = productDto.Name,
            SKU = productDto.SKU,
            Weight = productDto.Weight
        };
        await _productRepository.Create(product);
    }

    private async Task<bool> CheckProductExist(string productSKU)
    {
        var products = await _productRepository.Read();
        return products.Any(product => product.SKU == productSKU);
    }

    public async Task<Product> FindProduct(Guid productId)
    {
        var products = await _productRepository.Read();
        var product = products.FirstOrDefault(product => product.Id == productId);
        if (product != null)
        {
            return product;
        }

        throw new ValidationException();
    }
}
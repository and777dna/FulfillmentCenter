using System.ComponentModel.DataAnnotations;
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<PagedResult<ResponseProductDto>> GetProducts(QueryParams productsQueryParams)
    {
        var products = await productRepository.ReadAsync(productsQueryParams);
        
        
        return products;
    }

    public async Task CreateProduct(RequestProductDto productDto)
    {
        await CheckProductExist(productDto.SKU);
        
        Product product = new Product
        {
            Id = Guid.NewGuid(),
            Name = productDto.Name,
            SKU = productDto.SKU,
            Weight = productDto.Weight
        };
        await productRepository.CreateAsync(product);
    }

    private async Task CheckProductExist(string productSku)
    {
        var products = await productRepository.ReadAsync();
        var productAlreadyExist = products.Any(product => product.SKU == productSku);
        if (productAlreadyExist)
        {
            throw new InvalidOperationException("Запись с таким SKU уже существует в базе данных.");
        }
    }

    public async Task<Product> FindProduct(Guid productId)
    {
        var products = await productRepository.ReadAsync();
        var product = products.FirstOrDefault(product => product.Id == productId);
        if (product != null)
        {
            return product;
        }

        throw new ValidationException("no product was found");
    }
}
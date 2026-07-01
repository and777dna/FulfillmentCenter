using System.ComponentModel.DataAnnotations;
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;
using FulfillmentCenter.Services.MapperDto.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class ProductService(IRepository<Product> productRepository, IMapper<Product, ResponseProductDto> productMapper) : IProductService
{
    public async Task<PagedResult<ResponseProductDto>> GetProducts(QueryParams productsQueryParams)
    {
        var products = await productRepository.GetAllAsync();
        var productDtos = productMapper.ToDto(products);
        var pagedResult = productMapper.ToPagedResult(productsQueryParams.Page, productsQueryParams.PageSize, productDtos);
        return pagedResult;
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
        await productRepository.AddAsync(product);
    }

    private async Task CheckProductExist(string productSku)
    {
        var products = await productRepository.GetAllAsync();
        var productAlreadyExist = products.Any(product => product.SKU == productSku);
        if (productAlreadyExist)
        {
            throw new InvalidOperationException("Запись с таким SKU уже существует в базе данных.");
        }
    }

    public async Task<Product> FindProduct(Guid productId)
    {
        var products = await productRepository.GetAllAsync();
        var product = products.FirstOrDefault(product => product.Id == productId);
        if (product != null)
        {
            return product;
        }

        throw new ValidationException("no product was found");
    }
}
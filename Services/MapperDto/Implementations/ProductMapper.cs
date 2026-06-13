using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Services.MapperDto.Interfaces;

namespace FulfillmentCenter.Services.MapperDto.Implementations;

public class ProductMapper : IMapper<Product, ResponseProductDto>
{
    public List<ResponseProductDto> ToDto(List<Product> products)
    {
        var responseProductDtos = products.Select(product =>
            new ResponseProductDto
            {
                Name = product.Name,
                SKU = product.SKU,
                Weight = product.Weight
            }).ToList();
        return responseProductDtos;
    }
}
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

    
    /*PagedResult<ResponseProductDto> pagedResult = new PagedResult<ResponseProductDto>()
       {
           Page = page,
           PageSize = pageSize,
           TotalCount = products.Count,
           TotalPages = products.Count / page,
           Items = responseProductDtos
       };*/
    public PagedResult<ResponseProductDto> ToPagedResult(int page, int pageSize, List<ResponseProductDto> responseProductDtos)
    {
        var pagedResult = new PagedResult<ResponseProductDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = responseProductDtos.Count,
            TotalPages = responseProductDtos.Count / page,
            Items = responseProductDtos
        };

        return pagedResult;
    }
}
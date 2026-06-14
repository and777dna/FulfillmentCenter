using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Services.MapperDto.Interfaces;
using ResponseOrderDto = FulfillmentCenter.DTOs.Responses.ResponseOrderDto;

namespace FulfillmentCenter.Services.MapperDto.Implementations;

public class OrderMapper : IMapper<Order, ResponseOrderDto>
{
    public List<ResponseOrderDto> ToDto(List<Order> orders)
    {
        var responseOrderDtos = orders.Select(order => new ResponseOrderDto
        {
            CustomerId = order.CustomerId,
            DeliveryAddress = order.DeliveryAddress,
            CreatedAt = order.CreatedAt,
            Status = order.Status
        }).ToList();
        return responseOrderDtos;
    }
    
    public ResponseOrderDto ToDto(Order order)
    {
        return new ResponseOrderDto
        {
            CustomerId = order.CustomerId,
            DeliveryAddress = order.DeliveryAddress,
            CreatedAt = order.CreatedAt,
            Status = order.Status
        };
    }
    
    public PagedResult<ResponseOrderDto> ToPagedResult(int page, int pageSize, List<ResponseOrderDto> responseOrderDtos)
    {
        var pagedResult = new PagedResult<ResponseOrderDto>()
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = responseOrderDtos.Count,
            TotalPages = responseOrderDtos.Count / page,
            Items = responseOrderDtos
        };

        return pagedResult;
    }
}
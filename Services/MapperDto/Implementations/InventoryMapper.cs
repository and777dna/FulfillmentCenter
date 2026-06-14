using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Services.MapperDto.Interfaces;

namespace FulfillmentCenter.Services.MapperDto.Implementations;

public class InventoryMapper : IMapper<Inventory, ResponseInventoryDto>
{
    public List<ResponseInventoryDto> ToDto(List<Inventory> inventories)
    {
        List<ResponseInventoryDto> remainingsPdo = inventories.Select(remain => new ResponseInventoryDto
        {
            ProductId = remain.ProductId,
            Quantity = remain.Quantity
        }).ToList();
        return remainingsPdo;
    }

    public PagedResult<ResponseInventoryDto> ToPagedResult(int page, int pageSize, List<ResponseInventoryDto> responseDtos)
    {
        throw new NotImplementedException();
    }
}
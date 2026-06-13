using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Services.MapperDto.Interfaces;

public interface IMapper<TEntity, TDto>
{
    public List<TDto> ToDto(List<TEntity> items);
}
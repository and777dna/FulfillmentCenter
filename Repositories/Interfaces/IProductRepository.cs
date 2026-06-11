using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Repositories.Interfaces;

public interface IProductRepository
{
    public Task CreateAsync(Product product);
    public Task DeleteAsync(Guid id);
    public Task<PagedResult<ResponseProductDto>> ReadAsync(QueryParams productsQueryParams);
    public Task<List<Product>> ReadAsync();
}
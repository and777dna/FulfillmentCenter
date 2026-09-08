using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Services.Interfaces;

public interface IProductService
{
    public Task<PagedResult<ResponseProductDto>> GetProducts(QueryParams productsQueryParams);
    public Task CreateProduct(RequestProductDto product);
    public Task<Product> FindProduct(Guid productId);
}
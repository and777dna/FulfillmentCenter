using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Entities.Operation.Interfaces;

namespace FulfillmentCenter.Services.Interfaces;

public interface IInventoryService
{
    public Task AddStock(RequestInventoryDto inventoryDto, Guid fulfillmentCenterId);
    public Task<PagedResult<ResponseInventoryDto>> RemainingsOnTheFulfillmentCenter(Guid centerId,
        QueryParams queryParams);
    public Task UpdateInventoryProduct(Guid productId, IOperation<Inventory> operation, Guid centerId);
}
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Services.Interfaces;

public interface IInventoryService
{
    public Task AddStock(RequestInventoryDto inventoryDto, Guid fulfillmentCenterId);
    public Task<PagedResult<ResponseInventoryDto>> RemainingsOnTheFulfillmentCenter(Guid centerId,
        QueryParams queryParams);
    public Dictionary<Guid, int> ReturnProductAmount(ICollection<Inventory> inventories);
    public Task UpdateInventoryProduct(Guid productId, int quantity, Guid centerId);
}
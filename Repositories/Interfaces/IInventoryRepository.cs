using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Repositories.Interfaces;

public interface IInventoryRepository : IRepository<Inventory>
{
    public Task<List<Inventory>> ReadAsync(QueryParams queryParams);
    public Task UpdateInventoryAsync(Inventory inventory);
    public Task UpdateInventoryQuantityAsync(UpdateInventoryDto inventory);
}
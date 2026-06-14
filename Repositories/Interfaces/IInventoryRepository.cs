using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Repositories.Interfaces;

public interface IInventoryRepository
{
    public Task CreateAsync(Inventory inventory);
    public Task DeleteAsync(Guid id);
    public Task<List<Inventory>> ReadAsync();
    public Task<List<Inventory>> ReadAsync(QueryParams queryParams);
    public Task UpdateInventoryAsync(Inventory inventory);
    public Task UpdateInventoryQuantityAsync(UpdateInventoryDto inventory);
}
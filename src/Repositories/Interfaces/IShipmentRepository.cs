using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;

namespace FulfillmentCenter.Repositories.Interfaces;

public interface IShipmentRepository
{
    public Task CreateAsync(Shipment shipment);
    public Task DeleteAsync(Guid id);
    public Task<List<Shipment>> ReadAsync();
    public Task UpdateShipmentAsync<TUpdateParameter>(Guid id, TUpdateParameter updateParameter,
        Action<TUpdateParameter, Shipment> up);
    public Task UpdateShipmentStatusAsync(Guid shipmentId, ShipmentStatus status);
}
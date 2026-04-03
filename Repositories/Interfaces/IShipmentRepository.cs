using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;

namespace FulfillmentCenter.Repositories.Interfaces;

public interface IShipmentRepository
{
    public Task Create(Shipment shipment);
    public Task Delete(Guid id);
    public Task<List<Shipment>> Read();
    //public void UpdateShipment(Guid id);
    public Task UpdateShipment<TUpdateParameter>(Guid id, TUpdateParameter updateParameter,
        Action<TUpdateParameter, Shipment> up);
    public Task UpdateShipmentStatus(Guid shipmentId, ShipmentStatus status);
}
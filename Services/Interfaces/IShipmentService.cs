using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;

namespace FulfillmentCenter.Services.Interfaces;

public interface IShipmentService
{
    public void CreateShipment(Shipment shipment);

    public void UpdateShipmentStatus(Guid shipmentId, ShipmentStatus status);
}
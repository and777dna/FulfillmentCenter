using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Services.Interfaces;

public interface IShipment
{
    public void CreateShipment(Shipment shipment);
}
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;

namespace FulfillmentCenter.Services.Interfaces;

public interface IShipmentService
{
    public void CreateShipment(RequestShipmentDto requestShipmentDto);

    public void UpdateShipmentStatus(Guid shipmentId, ShipmentStatus status);
}
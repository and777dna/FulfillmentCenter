using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;

namespace FulfillmentCenter.Services.Interfaces;

public interface IShipmentService
{
    public Task CreateShipment(RequestShipmentDto requestShipmentDto);

    public void UpdateShipmentStatus(Guid shipmentId, ShipmentStatus status);
}
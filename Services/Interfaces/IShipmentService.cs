using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;

namespace FulfillmentCenter.Services.Interfaces;

public interface IShipmentService
{
    public Task CreateShipment(RequestShipmentDto requestShipmentDto);

    public Task UpdateShipmentStatus(Guid shipmentId, ShipmentStatus status);
}
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Implementations;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class ShipmentService(IShipmentRepository shipmentRepository, IInventoryRepository inventoryRepository) : IShipmentService
{
    private IShipmentRepository _shipmentRepository = shipmentRepository;
    private IInventoryRepository _inventoryRepository = inventoryRepository;
    
    
    //TODO: DTOs here for _sqlShipmentRepository, _sqlInventoryRepository
    
    public void CreateShipment(Shipment shipment)
    {
        _shipmentRepository.Create(shipment);
    }

    public void UpdateShipmentStatus(Guid shipmentId, ShipmentStatus status)
    {
        _shipmentRepository.UpdateShipmentStatus(shipmentId, status);
    }
}
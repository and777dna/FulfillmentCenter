using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Implementations;
using FulfillmentCenter.Services.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class ShipmentService(SqlShipmentRepository sqlShipmentRepository, SqlInventoryRepository sqlInventoryRepository) : IShipment
{
    private SqlShipmentRepository _sqlShipmentRepository = sqlShipmentRepository;
    private SqlInventoryRepository _sqlInventoryRepository = sqlInventoryRepository;
    
    
    //TODO: DTOs here for _sqlShipmentRepository, _sqlInventoryRepository
    
    public void CreateShipment(Shipment shipment)
    {
        _sqlShipmentRepository.Create(shipment);
    }

    public void UpdateShipmentStatus(Guid shipmentId, ShipmentStatus status)
    {
        _sqlShipmentRepository.UpdateShipmentStatus(shipmentId, status);
    }
}
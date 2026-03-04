using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentCenter.Controllers;

//['/api/shipments']
    
[Route("/api/shipments")]
public class ShipmentsController(ShipmentService shipmentService) : Controller
{
    private ShipmentService _shipmentService = shipmentService;
    
    
    [HttpPost]
    public void CreateShipment(Shipment shipment)
    {
        _shipmentService.CreateShipment(shipment);
        UpdateShipmentStatus(shipment.Id, ShipmentStatus.Shipped);
    }
    
    
    [HttpPut("${id}/status")]
    public void UpdateShipmentStatus(Guid shipmentId, ShipmentStatus status)
    {
        _shipmentService.UpdateShipmentStatus(shipmentId, status);
    }
    
}
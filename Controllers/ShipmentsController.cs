using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Services.Implementations;
using FulfillmentCenter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentCenter.Controllers;
    
[ApiController]
[Route("/api/shipments")]
public class ShipmentsController(IShipmentService shipmentService) : ControllerBase
{
    private IShipmentService _shipmentService = shipmentService;
    
    
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
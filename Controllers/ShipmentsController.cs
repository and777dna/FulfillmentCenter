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
        //TODO review: this invokes the controller action as a plain C# method, bypassing the HTTP pipeline entirely. Should be to call _shipmentService.UpdateShipmentStatus(...) directly instead
        UpdateShipmentStatus(shipment.Id, ShipmentStatus.Shipped);
    }
    
    //TODO review: you don't need to use $ in the route. It's not valid
    //The route template uses {id} but the method parameter is named shipmentId. These must match for route binding to work. It should be either {shipmentId} in the route or Guid id in the parameter
    [HttpPut("${id}/status")]
    public void UpdateShipmentStatus(Guid shipmentId, ShipmentStatus status)
    {
        _shipmentService.UpdateShipmentStatus(shipmentId, status);
    }
    
}
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Implementations;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class ShipmentService(IShipmentRepository shipmentRepository, IInventoryRepository inventoryRepository, IInventoryService inventoryService) : IShipmentService
{
    private IShipmentRepository _shipmentRepository = shipmentRepository;
    private IInventoryRepository _inventoryRepository = inventoryRepository;
    private IInventoryService _inventoryService = inventoryService;
    
    
    //TODO: DTOs here for _sqlShipmentRepository, _sqlInventoryRepository
    
    public Dictionary<Guid, int> ReturnShipmentAmount(ICollection<OrderItem> shipmentAmount)
    {//ICollection<OrderItem> Items
        Dictionary<Guid, int> openWith = new Dictionary<Guid, int>();
        foreach (var shipment in shipmentAmount)
        {
            openWith.Add(shipment.ProductId, shipment.Quantity);
        }

        return openWith;
    }

    public bool CheckSufficientAmountOfInventoryToShipment(Dictionary<Guid, int> ReturnProductAmount, Dictionary<Guid, int> ReturnShipmentAmount)
    {
        ReturnProductAmount.OrderBy(product => product.Key);
        return true;
    }
    
    public void CreateShipment(RequestShipmentDto requestShipmentDto)
    {//na FulfillmentCenter достаточно товара для каждой позиции Order
        var remainingsOnTheFulfillmentCenter = _inventoryService.RemainingsOnTheFulfillmentCenter(requestShipmentDto.DistributionCenterId);
        //Inventory.DistributionCenterId .Inventory => forEach()
        //Shipment.Order ICollection<OrderItem> Items => 
        //
        //var sufficientAmountOfInventory = _inventoryService.CheckSufficientAmountOfInventory(remainingsOnTheFulfillmentCenter, shipment.Order.Items);
        var shipment =
            new Shipment {
                Id = requestShipmentDto.Id,
                OrderId = requestShipmentDto.OrderId,
                Order = requestShipmentDto.Order,
                DistributionCenterId = requestShipmentDto.DistributionCenterId,
                DistributionCenter = requestShipmentDto.DistributionCenter,
                Status = requestShipmentDto.Status,
                ShippedAt = requestShipmentDto.ShippedAt,
                EstimatedDelivery = requestShipmentDto.EstimatedDelivery
            };
        if (CheckSufficientAmountOfInventoryToShipment(_inventoryService.ReturnProductAmount(remainingsOnTheFulfillmentCenter.Result), ReturnShipmentAmount(shipment.Order.Items)))
        {
            _shipmentRepository.Create(shipment);
        }
        _shipmentRepository.Create(shipment);
    }
    
    

    public void UpdateShipmentStatus(Guid shipmentId, ShipmentStatus status)
    {
        _shipmentRepository.UpdateShipmentStatus(shipmentId, status);
    }
}
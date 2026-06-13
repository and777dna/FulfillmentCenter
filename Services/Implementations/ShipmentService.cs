using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Implementations;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class ShipmentService(IShipmentRepository shipmentRepository, IInventoryRepository inventoryRepository, IInventoryService inventoryService, IOrderRepository orderRepository, IFulfillmentCenterService fulfillmentCenterService) : IShipmentService
{
    private IShipmentRepository _shipmentRepository = shipmentRepository;
    private IInventoryRepository _inventoryRepository = inventoryRepository;
    private IInventoryService _inventoryService = inventoryService;
    private IOrderRepository _orderRepository = orderRepository;
    private IFulfillmentCenterService _fulfillmentCenterService = fulfillmentCenterService;
    
    
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
    
    public async Task CreateShipment(RequestShipmentDto requestShipmentDto)
    {//na FulfillmentCenter достаточно товара для каждой позиции Order
        //TODO: to update remainingsOnTheFulfillmentCenter
        //var remainingsOnTheFulfillmentCenter = _inventoryService.RemainingsOnTheFulfillmentCenter(requestShipmentDto.DistributionCenterId);
        //Inventory.DistributionCenterId .Inventory => forEach()
        //Shipment.Order ICollection<OrderItem> Items => 
        //
        //var sufficientAmountOfInventory = _inventoryService.CheckSufficientAmountOfInventory(remainingsOnTheFulfillmentCenter, shipment.Order.Items);
        var order = await _orderRepository.GetOrderById(requestShipmentDto.OrderId);
        var distributionCenter = await _fulfillmentCenterService.FindFulfillmentCenter(requestShipmentDto.DistributionCenterId);
        
        var shipment =
            new Shipment {
                Id = Guid.NewGuid(),
                OrderId = requestShipmentDto.OrderId,
                DistributionCenterId = requestShipmentDto.DistributionCenterId,
                Status = requestShipmentDto.Status,
                ShippedAt = DateTime.UtcNow,
                EstimatedDelivery = DateTime.UtcNow
            };
        
        /*if (CheckSufficientAmountOfInventoryToShipment(_inventoryService.ReturnProductAmount(remainingsOnTheFulfillmentCenter.Result), ReturnShipmentAmount(shipment.Order.Items)))
        {
            _shipmentRepository.Create(shipment);
        }*/
        await _shipmentRepository.CreateAsync(shipment);
    }
    
    

    public async Task UpdateShipmentStatus(Guid shipmentId, ShipmentStatus status)
    {
        await _shipmentRepository.UpdateShipmentStatusAsync(shipmentId, status);
    }
}
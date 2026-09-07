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
    {
        Dictionary<Guid, int> openWith = new Dictionary<Guid, int>();
        foreach (var shipment in shipmentAmount)
        {
            openWith.Add(shipment.ProductId, shipment.Quantity);
        }

        return openWith;
    }
    
    public async Task CreateShipment(RequestShipmentDto requestShipmentDto)
    {
        var order = await _orderRepository.GetByIdAsync(requestShipmentDto.OrderId);
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
        
        await _shipmentRepository.CreateAsync(shipment);
    }
    
    

    public async Task UpdateShipmentStatus(Guid shipmentId, ShipmentStatus status)
    {
        await _shipmentRepository.UpdateShipmentStatusAsync(shipmentId, status);
    }
}
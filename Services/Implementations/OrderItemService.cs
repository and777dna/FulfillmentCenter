using System.Transactions;
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class OrderItemService(IOrderItemRepository orderItemRepository, IInventoryService inventoryService) : IOrderItemService
{
    private IOrderItemRepository _orderItemRepository = orderItemRepository;
    private readonly IInventoryService _inventoryService = inventoryService;
    
    public async Task AddOrderItemToOrder(RequestOrderItemDto? orderItemDto, Guid centerId)
    {
        if (orderItemDto == null)
        {
            throw new ArgumentNullException(nameof(orderItemDto), "orderItemDto is null");
        }

        OrderItem orderItem = new OrderItem
        {
            Id = Guid.NewGuid(),
            OrderId = orderItemDto.OrderId,
            PricePerUnit = orderItemDto.PricePerUnit,
            ProductId = orderItemDto.ProductId,
            Quantity = orderItemDto.Quantity
        };
        
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        
        await _orderItemRepository.Create(orderItem);
        await _inventoryService.UpdateInventoryProduct(orderItemDto.ProductId, orderItemDto.Quantity, centerId);
        
        scope.Complete();
    }
    
    
}
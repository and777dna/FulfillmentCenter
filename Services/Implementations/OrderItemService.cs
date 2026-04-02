using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class OrderItemService(IOrderItemRepository orderItemRepository) : IOrderItemService
{
    private IOrderItemRepository _orderItemRepository = orderItemRepository;
    
    public async Task AddOrderItemToOrder(RequestOrderItemDto? orderItemDto)
    {
        if (orderItemDto == null)
        {
            throw new ArgumentNullException(nameof(orderItemDto));
        }

        OrderItem orderItem = new OrderItem
        {
            Id = Guid.NewGuid(),
            OrderId = orderItemDto.OrderId,
            PricePerUnit = orderItemDto.PricePerUnit,
            ProductId = orderItemDto.ProductId,
            Quantity = orderItemDto.Quantity
        };
        await _orderItemRepository.Create(orderItem);
    }
    
    
}
using FulfillmentCenter.Data;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Interfaces;

namespace FulfillmentCenter.Services.Handlers.Implementations;

public class OrderCancelHandler(IOrderRepository orderRepository, FulfillmentCenDbContext context, ILogger<OrderCancelHandler> logger) : IOrderStatusHandler
{
    private IOrderRepository _orderRepository = orderRepository;

    public OrderStatus SupportedStatus => OrderStatus.Cancelled;
    
    public async Task HandleAsync(Guid orderId)
    {
        await _orderRepository.UpdateOrder(SupportedStatus, orderId, (order, status) => { order.Status = status;});
        await _orderRepository.Delete(orderId);
        
        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "not possible to save updated status and afterwards soft deleted order");
            throw;
        }
    }
}
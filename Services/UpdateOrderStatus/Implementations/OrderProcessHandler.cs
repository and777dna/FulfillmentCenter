using FulfillmentCenter.Data;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Handlers.Implementations;

namespace FulfillmentCenter.Services.UpdateOrderStatus.Implementations;

public class OrderProcessHandler(FulfillmentCenDbContext context, IOrderRepository orderRepository, ILogger<OrderDeliverHandler> logger)
{
    private IOrderRepository _orderRepository = orderRepository;

    public OrderStatus SupportedStatus => OrderStatus.Processing;
    
    public async Task HandleAsync(Guid orderId)
    {
        await _orderRepository.UpdateOrder(SupportedStatus, orderId, (order, status) => { order.Status = status;});
        
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
using System.Transactions;
using FulfillmentCenter.Data;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Interfaces;

namespace FulfillmentCenter.Services.UpdateOrderStatus.Implementations;

public class OrderDeliverHandler(FulfillmentCenDbContext context, IOrderRepository orderRepository, ILogger<OrderDeliverHandler> logger)
{
    private IOrderRepository _orderRepository = orderRepository;

    public OrderStatus SupportedStatus => OrderStatus.Cancelled;
    
    public async Task HandleAsync(Guid orderId)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        
        await _orderRepository.UpdateOrder(SupportedStatus, orderId, (order, status) => { order.Status = status;});
        await _orderRepository.Delete(orderId);
        
        scope.Complete();
        
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
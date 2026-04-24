using FulfillmentCenter.Data;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Interfaces;

namespace FulfillmentCenter.Services.Handlers.Implementations;

public class OrderProcessHandler(FulfillmentCenDbContext context, IOrderRepository orderRepository)
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
            Console.WriteLine(e);
            throw;
        }
    }
}
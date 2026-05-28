using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlOrderItemRepository(FulfillmentCenDbContext context, ILogger<SqlOrderItemRepository> logger) : IOrderItemRepository
{
    int page = 2;
    int pageSize = 50;

    public async Task CreateAsync(OrderItem orderItem)
    {
        try
        {
            await context.OrderItems.AddAsync(orderItem);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "not possible to create an order item.");
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var orderItemToDelete = await context.OrderItems.FirstOrDefaultAsync(order => order.Id == id);
        if(orderItemToDelete == null)
        {
            throw new ArgumentNullException(nameof(id), "orderItem was not found");
        }

        try
        {
            orderItemToDelete.IsDeleted = true;
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "not possible to delete an order item");
            throw;
        }
    }

    public async Task<List<OrderItem>> ReadAsync()
    {
        //TODO: to add .Where(orderItem => order.orderItem != true)
         // order.Status != OrderStatus.Cancelled)
            List<OrderItem> orderItems = await context.OrderItems.Skip((page - 1) * pageSize)
                .Take(pageSize).OrderBy(p => p.Id).ToListAsync();
            return orderItems;
    }

    public async Task UpdateOrderItemAsync(){}
}
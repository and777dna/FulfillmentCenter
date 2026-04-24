using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlOrderRepository(FulfillmentCenDbContext context, ILogger<SqlOrderRepository> logger) : IOrderRepository
{
    int page = 2;
    int pageSize = 50;

    public async Task Create(Order order)
    {
        try
        {
            await context.Orders.AddAsync(order);
        }
        catch (Exception e)
        {
            logger.LogError(e, "not possible to create an order.");
            throw;
        }

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "not possible to save an order.");
            throw;
        }
    }

    public async Task Delete(Guid id)
    {
        var orderToDelete = await context.Orders.FirstOrDefaultAsync(order => order.Id == id);
        if(orderToDelete != null){orderToDelete.IsDeleted = true;}
        else
        {
            throw new ArgumentNullException(nameof(id), "no Order was found");
        }
        
    }

    public async Task<List<Order>> Read()
    {
            try
            {
                List<Order> orders = await context.Orders.Where(order => order.IsDeleted != true &&
                                                                          order.Status != OrderStatus.Cancelled)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .OrderBy(p => p.Id)
                    .ToListAsync();
                if (orders == null) throw new FileNotFoundException();
                return orders;
            }
            catch (Exception e)
            {
                logger.LogError(e, "not possible to read orders.");
                throw;
            }
    }
    
    public async Task UpdateOrder<TUpdateParam>(TUpdateParam updateParam,Guid orderId, Action<Order, TUpdateParam> up)
    {//.UpdateOrder(orderStatus, Id, (order, status) => { order.Status = status;});
        try
        {
            var orderToUpdate = await context.Orders.FirstOrDefaultAsync(order => order.Id == orderId);
            if (orderToUpdate == null)throw new KeyNotFoundException("orderToUpdate want found");
            up(orderToUpdate, updateParam);
        }
        catch (Exception e)
        {
            logger.LogError(e, "not possible to update orders.");
            throw;
        }
    }
}
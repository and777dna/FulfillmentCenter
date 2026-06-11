using FulfillmentCenter.Data;
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Filters;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlOrderRepository(FulfillmentCenDbContext context, ILogger<SqlOrderRepository> logger) : IOrderRepository
{
    public async Task CreateAsync(Order order)
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

    public async Task DeleteAsync(Guid id)
    {
        var orderToDelete = await context.Orders.FirstOrDefaultAsync(order => order.Id == id);
        if(orderToDelete != null){orderToDelete.IsDeleted = true;}
        else
        {
            throw new ArgumentNullException(nameof(id), "no Order was found");
        }
        
    }
    
    public async Task<List<Order>> ReadAsync()
    {
        
        try
        {
            List<Order> orders = await context.Orders
                //.Skip((page - 1) * pageSize)
                //.Take(pageSize)
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

    public async Task<List<Order>> ReadAsync(QueryParams queryParams)
    {
        var specification = new FilterBuilder(queryParams).Build();
        var page = queryParams.Page;
        var pageSize = queryParams.PageSize;
        
            try
            {
                List<Order> orders = await context.Orders
                    .Where(specification.ToExpression())
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
    
    public async Task UpdateOrderAsync<TUpdateParam>(TUpdateParam updateParam,Guid orderId, Action<Order, TUpdateParam> up)
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
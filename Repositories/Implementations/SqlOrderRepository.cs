using FulfillmentCenter.Data;
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.FilterV2.Implementations;
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
        
        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "not possible to delete an order.");
            throw;
        }
    }
    
    public async Task<List<Order>> ReadAsync()
    {
        try
        {
            List<Order> orders = await context.Orders
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

    public async Task<PagedResult<ResponseOrderDto>> ReadAsync(QueryParams queryParams)
    {
        var specification = new OrderFilterBuilder(context.Orders).DateRangeSpecification(queryParams.FromDate, queryParams.ToDate).Build();
        var page = queryParams.Page;
        var pageSize = queryParams.PageSize;
        
            try
            {
                List<Order> orders = await specification
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .OrderBy(p => p.Id)
                    .ToListAsync();
                
                List<ResponseOrderDto> responseOrderDtos = orders.Select(order => new ResponseOrderDto()
                {
                    CustomerId = order.CustomerId,
                    CreatedAt = order.CreatedAt,
                    DeliveryAddress = order.DeliveryAddress,
                    Status = order.Status
                }).ToList();

                PagedResult<ResponseOrderDto> pagedResult = new PagedResult<ResponseOrderDto>()
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = orders.Count, 
                    TotalPages = (int)Math.Ceiling((double)orders.Count / pageSize),
                    Items = responseOrderDtos
                };
                if (orders == null) throw new FileNotFoundException();
                return pagedResult;
            }
            catch (Exception e)
            {
                logger.LogError(e, "not possible to read orders.");
                throw;
            }
    }
    
    public async Task UpdateOrderAsync<TUpdateParam>(TUpdateParam updateParam,Guid orderId, Action<Order, TUpdateParam> up)
    {
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

    public async Task<Order> GetOrderById(Guid orderId)
    {
        var orders = await ReadAsync();
        
        var findBook = SearchById(orderId, orders);
        return findBook;
    }
    
    private Order SearchById(Guid orderId, List<Order> orders)
    {
        var findOrder = orders.FirstOrDefault(order => order.Id == orderId);
        if (findOrder != null)
        {
            return findOrder;
        }

        throw new ArgumentNullException(nameof(orderId), "Order not found");
    }
}
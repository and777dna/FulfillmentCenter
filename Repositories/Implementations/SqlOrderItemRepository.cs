using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlOrderItemRepository : IOrderItemRepository
{
    private readonly FulfillmentCenDbContext _context;
    int page = 2;
    int pageSize = 50;
    public SqlOrderItemRepository(FulfillmentCenDbContext context)
    {
        _context = context;
    }

    public async Task Create(OrderItem orderItem)
    {
        try
        {
            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task Delete(Guid id)
    {
        var orderItemToDelete = await _context.OrderItems.FirstOrDefaultAsync(order => order.Id == id);
        if(orderItemToDelete == null)
        {
            throw new ArgumentNullException(nameof(id), "orderItem was not found");
        }

        try
        {
            orderItemToDelete.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<OrderItem>> Read()
    {
        //TODO: to add .Where(orderItem => order.orderItem != true)
         // order.Status != OrderStatus.Cancelled)
            List<OrderItem> orderItems = await _context.OrderItems.Skip((page - 1) * pageSize)
                .Take(pageSize).OrderBy(p => p.Id).ToListAsync();
            return orderItems;
    }

    public async Task UpdateOrderItem(){}
}
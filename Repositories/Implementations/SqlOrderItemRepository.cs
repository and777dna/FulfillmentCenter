using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlOrderItemRepository : IOrderItemRepository
{
    private FulfillmentCenDbContext _context;
    public List<OrderItem> OrderItems;
    private bool _isCached;//TODO: to remember this is the In-Memory Cache techique
    
    public SqlOrderItemRepository(FulfillmentCenDbContext context)
    {
        _context = context;
        OrderItems = Read().Result;
        _isCached = true;
    }

    public async Task Create(OrderItem orderItem)
    {
        try
        {
            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
            _isCached = false;
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
            throw new ArgumentNullException();
        }

        try
        {
            _context.OrderItems.Remove(orderItemToDelete);
            await _context.SaveChangesAsync();
            _isCached = false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<OrderItem>> Read()
    {
        if (_isCached == false)
        {//TODO: to add .Where(orderItem => order.orderItem != true)
         // order.Status != OrderStatus.Cancelled)
            List<OrderItem> orderItems = await _context.OrderItems.ToListAsync();
            _isCached = true;
            return orderItems;
        }

        return OrderItems;
    }

    public async Task UpdateOrderItem(){}
}
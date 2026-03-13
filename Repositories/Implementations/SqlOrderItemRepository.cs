using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlOrderItemRepository : IOrderItemRepository
{
    private FulfillmentCenDbContext _context;
    public List<OrderItem> OrderItems;
    private bool isCached;
    
    public SqlOrderItemRepository(FulfillmentCenDbContext context)
    {
        _context = context;
        OrderItems = Read().Result;
        isCached = true;
    }

    public async void Create(OrderItem orderItem)
    {
        await _context.OrderItems.AddAsync(orderItem);
        await _context.SaveChangesAsync();
    }

    public async void Delete(Guid id)
    {
        var orderItemToDelete = await _context.OrderItems.FirstOrDefaultAsync(order => order.Id == id);
        _context.OrderItems.Remove(orderItemToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<List<OrderItem>> Read()
    {
        if (isCached == false)
        {
            List<OrderItem> orderItems = await _context.OrderItems.ToListAsync();
            isCached = true;
            return orderItems;
        }

        return OrderItems;
    }
    public void UpdateOrderItem(){}
}
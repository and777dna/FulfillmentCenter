using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlOrderRepository : IOrderRepository
{
    private FulfillmentCenDbContext _context;
    //public List<Order> Orders;
    //private bool isCached;
    
    public SqlOrderRepository(FulfillmentCenDbContext context)
    {
        _context = context;
        //Orders = Read().Result;
        //isCached = true;
    }
    public async Task Create(Order order)
    {
        try
        {
            await _context.Orders.AddAsync(order);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async void Delete(Guid id)
    {
        var orderToDelete = await _context.Orders.FirstOrDefaultAsync(order => order.Id == id);
        _context.Orders.Remove(orderToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Order>> Read()
    {
        //if (isCached == false)
        //{
        try
        {
            List<Order> orders = await _context.Orders.ToListAsync();
            //isCached = true;
            return orders;
            //}
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        //return Orders;
    }
    
    public async void UpdateOrder<TUpdateParam>(TUpdateParam updateParam,Guid orderId, Action<Order, TUpdateParam> up)
    {
        var orderToUpdate = await _context.Orders.FirstOrDefaultAsync(order => order.Id == orderId);
        up(orderToUpdate, updateParam);
        await _context.SaveChangesAsync();
    }
}
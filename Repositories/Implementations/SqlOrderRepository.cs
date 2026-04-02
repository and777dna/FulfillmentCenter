using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlOrderRepository : IOrderRepository
{
    private FulfillmentCenDbContext _context;
    public List<Order> Orders;
    private bool _isCached;
    
    public SqlOrderRepository(FulfillmentCenDbContext context)
    {
        _context = context;
        Orders = Read().Result;
        _isCached = true;
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
        var orderToDelete = await _context.Orders.FirstOrDefaultAsync(order => order.Id == id);
        if(orderToDelete != null){orderToDelete.IsDeleted = true;}
        else
        {
            throw new ArgumentNullException();
        }

        try
        {
            await _context.SaveChangesAsync();
            _isCached = false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<Order>> Read()
    {
        if (_isCached == false)
        {
            try
            {
                List<Order> orders = await _context.Orders.Where(order => order.IsDeleted != true &&
                                                                          order.Status != OrderStatus.Cancelled)
                    .ToListAsync();
                _isCached = true;
                return orders;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        return Orders;
    }
    
    public async Task UpdateOrder<TUpdateParam>(TUpdateParam updateParam,Guid orderId, Action<Order, TUpdateParam> up)
    {//.UpdateOrder(orderStatus, Id, (order, status) => { order.Status = status;});
        try
        {
            var orderToUpdate = await _context.Orders.FirstOrDefaultAsync(order => order.Id == orderId);
            up(orderToUpdate, updateParam);
            await _context.SaveChangesAsync();
            _isCached = false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
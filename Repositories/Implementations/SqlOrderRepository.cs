using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlOrderRepository : IOrderRepository
{
    private readonly FulfillmentCenDbContext _context;
    int page = 2;
    int pageSize = 50;
    public SqlOrderRepository(FulfillmentCenDbContext context)
    {
        _context = context;
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

    public async Task Delete(Guid id)
    {
        var orderToDelete = await _context.Orders.FirstOrDefaultAsync(order => order.Id == id);
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
                List<Order> orders = await _context.Orders.Where(order => order.IsDeleted != true &&
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
                Console.WriteLine(e);
                throw;
            }
    }
    
    public async Task UpdateOrder<TUpdateParam>(TUpdateParam updateParam,Guid orderId, Action<Order, TUpdateParam> up)
    {//.UpdateOrder(orderStatus, Id, (order, status) => { order.Status = status;});
        try
        {
            var orderToUpdate = await _context.Orders.FirstOrDefaultAsync(order => order.Id == orderId);
            if (orderToUpdate == null)throw new KeyNotFoundException("orderToUpdate want found");
            up(orderToUpdate, updateParam);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
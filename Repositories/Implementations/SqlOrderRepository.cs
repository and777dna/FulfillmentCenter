using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Implementations;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlOrderRepository : IOrderRepository
{
    private FulfillmentCenDbContext _context;
    public List<Order> Orders;
    private bool isCached;
    
    public SqlOrderRepository()
    {
        _context = new FulfillmentCenDbContext();
        Orders = Read();
        isCached = true;
    }
    public void Create(Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var orderToDelete = _context.Orders.FirstOrDefault(order => order.Id == id);
        _context.Orders.Remove(orderToDelete);
        _context.SaveChanges();
    }

    public List<Order> Read()
    {
        if (isCached == false)
        {
            List<Order> orders = _context.Orders.ToList();
            isCached = true;
            return orders;
        }

        return Orders;
    }
    public void UpdateOrder(){}
}
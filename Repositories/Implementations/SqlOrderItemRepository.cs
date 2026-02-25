using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlOrderItemRepository : IOrderItemRepository
{
    private FulfillmentCenDbContext _context;
    public List<OrderItem> OrderItems;
    private bool isCached;
    
    public SqlOrderItemRepository()
    {
        _context = new FulfillmentCenDbContext();
        OrderItems = Read();
        isCached = true;
    }

    public void Create(OrderItem orderItem)
    {
        _context.OrderItems.Add(orderItem);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var orderItemToDelete = _context.OrderItems.FirstOrDefault(order => order.Id == id);
        _context.OrderItems.Remove(orderItemToDelete);
        _context.SaveChanges();
    }

    public List<OrderItem> Read()
    {
        if (isCached == false)
        {
            List<OrderItem> orderItems = _context.OrderItems.ToList();
            isCached = true;
            return orderItems;
        }

        return OrderItems;
    }
    public void UpdateOrderItem(){}
}
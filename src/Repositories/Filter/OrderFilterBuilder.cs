using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Repositories.Filter;

public class OrderFilterBuilder(IQueryable<Order> orders)
{
    public OrderFilterBuilder DateRangeSpecification(DateTime? from, DateTime? to)
    {
        orders = orders.Where(order => order.CreatedAt > from);
        return this;
    }
    public IQueryable<Order> Build()
    {
        return orders;
    }
}
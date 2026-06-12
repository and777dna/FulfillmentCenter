using System.Linq.Expressions;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Filters.Interfaces;

namespace FulfillmentCenter.Repositories.Filters.Implementations;

public sealed class DateRangeSpecification(
    DateTime from,
    DateTime? to) : Specification<Order>
{
    public override Expression<Func<Order, bool>> ToExpression()
    {
        return order =>
            order.CreatedAt >= from &&
            (!to.HasValue || order.CreatedAt <= to.Value);
    }
}
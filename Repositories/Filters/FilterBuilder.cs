using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Filters.Implementations;
using FulfillmentCenter.Repositories.Filters.Interfaces;

namespace FulfillmentCenter.Repositories.Filters;

public class FilterBuilder(OrderFilterParams filter)
{
    public ISpecification<Order> Build()
    {
        Specification<Order> spec = new TrueSpecification<Order>();

        if (filter.fromDate.HasValue)
        {
            //spec = spec.And(o => o.CreatedAt >= filter.fromDate.Value);
            spec = spec.And(new DateRangeSpecification(filter.fromDate.Value, filter.toDate));
        }

        return spec;
    }
}
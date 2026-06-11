using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Filters.Implementations;
using FulfillmentCenter.Repositories.Filters.Interfaces;

namespace FulfillmentCenter.Repositories.Filters;

public class FilterBuilder(QueryParams filter)
{
    public ISpecification<Order> Build()
    {
        Specification<Order> spec = new TrueSpecification<Order>();

        if (filter.FromDate.HasValue)
        {
            //spec = spec.And(o => o.CreatedAt >= filter.fromDate.Value);
            spec = spec.And(new DateRangeSpecification(filter.FromDate.Value, filter.ToDate));
        }

        return spec;
    }
}
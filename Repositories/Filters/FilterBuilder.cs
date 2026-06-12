using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Filters.Implementations;
using FulfillmentCenter.Repositories.Filters.Interfaces;

namespace FulfillmentCenter.Repositories.Filters;

public class FilterBuilder<T>(QueryParams filter)
{//TODO: to create this for each entity
    public ISpecification<T> Build()
    {
        Specification<T> spec = new TrueSpecification<T>();

        if (filter.FromDate.HasValue)
        {
            //spec = spec.And(o => o.CreatedAt >= filter.fromDate.Value);
            spec = spec.And(new DateRangeSpecification<T>(filter.FromDate.Value, filter.ToDate));
        }

        return spec;
    }
}
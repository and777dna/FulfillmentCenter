using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Repositories.FilterV2.Implementations;

public class ProductFilterBuilder(IQueryable<Product> products)
{
    public ProductFilterBuilder FilterWeight(decimal? fromWeight, decimal? toWeight)
    {
        products = products.Where(product => product.Weight > fromWeight);
        return this;
    }
    public IQueryable<Product> Build()
    {
        return products;
    }
}
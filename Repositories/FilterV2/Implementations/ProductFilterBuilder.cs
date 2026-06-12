using FulfillmentCenter.Entities;

namespace FulfillmentCenter.Repositories.FilterV2.Implementations;

public class ProductFilterBuilder(IQueryable<Product> products)
{
    private IQueryable<Product> _products = products;

    public ProductFilterBuilder FilterWeight(decimal? fromWeight, decimal? toWeight)
    {
        _products = _products.Where(product => product.Weight > fromWeight);
        return this;
    }
    public IQueryable<Product> Build()
    {
        return _products;
    }
}
using System.Linq.Expressions;

namespace FulfillmentCenter.Repositories.Filters.Implementations;

public sealed class TrueSpecification<T> : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        return _ => true;
    }
}
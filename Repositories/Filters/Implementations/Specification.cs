using System.Linq.Expressions;
using FulfillmentCenter.Repositories.Filters.Interfaces;

namespace FulfillmentCenter.Repositories.Filters.Implementations;

public abstract class Specification<T> : ISpecification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();

    public Specification<T> And(ISpecification<T> other)
    {
        return new AndSpecification<T>(this, other);
    }
}
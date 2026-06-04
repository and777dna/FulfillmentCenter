using System.Linq.Expressions;

namespace FulfillmentCenter.Repositories.Filters.Interfaces;

public interface ISpecification<T>
{
    //IQueryable<T> Apply();
    Expression<Func<T, bool>> ToExpression();
}
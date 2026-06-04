using System.Linq.Expressions;
using FulfillmentCenter.Repositories.Filters.Interfaces;

namespace FulfillmentCenter.Repositories.Filters.Implementations;

public sealed class AndSpecification<T>(
    ISpecification<T> left,
    ISpecification<T> right) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpr = left.ToExpression();
        var rightExpr = right.ToExpression();

        var parameter = Expression.Parameter(typeof(T));

        var leftBody = new ReplaceParameterVisitor(
                leftExpr.Parameters[0],
                parameter)
            .Visit(leftExpr.Body);

        var rightBody = new ReplaceParameterVisitor(
                rightExpr.Parameters[0],
                parameter)
            .Visit(rightExpr.Body);

        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(leftBody!, rightBody!),
            parameter);
    }
}
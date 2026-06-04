using System.Linq.Expressions;

namespace FulfillmentCenter.Repositories.Filters;

internal sealed class ReplaceParameterVisitor(
    ParameterExpression oldParameter,
    ParameterExpression newParameter)
    : ExpressionVisitor
{
    protected override Expression VisitParameter(ParameterExpression node)
    {
        return node == oldParameter
            ? newParameter
            : base.VisitParameter(node);
    }
}
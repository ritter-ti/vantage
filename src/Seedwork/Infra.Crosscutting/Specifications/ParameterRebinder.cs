using System.Collections.Generic;
using System.Linq.Expressions;

namespace Goal.Seedwork.Infra.Crosscutting.Specifications
{
    public sealed class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            => new ParameterRebinder(map).Visit(exp);

        protected override Expression VisitParameter(ParameterExpression node)
        {
            ParameterExpression replacement = map.GetValueOrDefault(node);
            node = replacement;

            return base.VisitParameter(node);
        }
    }
}

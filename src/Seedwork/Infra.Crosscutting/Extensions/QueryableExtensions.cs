using System.Linq;
using System.Linq.Expressions;

namespace Goal.Seedwork.Infra.Crosscutting.Extensions
{
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool ascending)
            => OrderingHelper(source, propertyName, !ascending, false);

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
            => OrderingHelper(source, propertyName, false, false);

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
            => OrderingHelper(source, propertyName, true, false);

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName)
            => OrderingHelper(source, propertyName, false, true);

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string propertyName)
            => OrderingHelper(source, propertyName, true, true);

        private static IOrderedQueryable<T> OrderingHelper<T>(IQueryable<T> source, string propertyName, bool descending, bool anotherLevel)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), string.Empty);
            MemberExpression property = null;
            LambdaExpression sort = null;

            foreach (string propName in propertyName.Split('.'))
            {
                property = Expression.Property((Expression)property ?? param, propName);
                sort = Expression.Lambda(property, param);
            }

            string level = !anotherLevel ? "OrderBy" : "ThenBy";
            string direction = descending ? "Descending" : string.Empty;

            MethodCallExpression call = Expression.Call(typeof(Queryable),
                                                        $"{level}{direction}",
                                                        new[] { typeof(T), property?.Type },
                                                        source.Expression,
                                                        Expression.Quote(sort));

            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
        }
    }
}

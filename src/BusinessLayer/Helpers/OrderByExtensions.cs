using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BusinessLayer.Helpers
{
    public static class OrderByExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName, IComparer<object> comparer = null)
        {
            return CallOrderedQueryable(query, "OrderBy", propertyName, comparer);
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string propertyName, IComparer<object> comparer = null)
        {
            return CallOrderedQueryable(query, "OrderByDescending", propertyName, comparer);
        }

        public static IOrderedQueryable<T> CallOrderedQueryable<T>(this IQueryable<T> query, string methodName, string propertyName,
            IComparer<object> comparer = null)
        {
            var param = Expression.Parameter(typeof(T), "x");

            var body = propertyName.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);

            return comparer != null
                ? (IOrderedQueryable<T>)query.Provider.CreateQuery(
                    Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new[] { typeof(T), body.Type },
                        query.Expression,
                        Expression.Lambda(body, param),
                        Expression.Constant(comparer)
                    )
                )
                : (IOrderedQueryable<T>)query.Provider.CreateQuery(
                    Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new[] { typeof(T), body.Type },
                        query.Expression,
                        Expression.Lambda(body, param)
                    )
                );
        }
    }
}
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SqlKata.Extensions
{
    using static Base;
    public static class Extensions
    {
        public static Query Select<T>(this Query query, params Expression<Func<T, object>>[] columnSelectors)
        {

            var columns = columnSelectors
                          .Select(GetColumn)
                          .ToArray();

            return query.Select(columns);
        }

        public static Query Select<T>(this Query query)
        {
            var columns = typeof(T).GetSelectProperties()
                                   .Select(p => p.Name)
                                   .ToArray();

            return query.Select(columns);
        }
    }
}
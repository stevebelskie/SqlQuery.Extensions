using System;
using System.Linq;
using System.Linq.Expressions;

namespace SqlKata.Extensions
{
    using static Base;
    public static class QueryTExtensions
    {
        public static Query<T> Select<T>(this Query<T> query, params Expression<Func<T, object>>[] columnSelectors)
        {
            var columns = columnSelectors.Select(GetColumn)
                                         .ToArray<string>();

            return query.Select(columns);

        }

        public static Query<T> Select<T>(this Query<T> query)
        {
            var columns = typeof(T).GetSelectProperties()
                                   .Select(p => p.Name)
                                   .ToArray();

            return query.Select(columns);
        }

        /// <summary>
        /// <para>
        /// By default the method adds where clauses ('MemberName'=@memberValue) for properties that are non-defaults
        /// </para>
        /// <para>
        /// This means <see><cref>default(T)</cref></see> (e.g. 0 for <see cref="int"/>
        /// is ignored where T is a value type, in addition to null values for reference types.
        /// </para>
        /// 
        /// <example>
        /// 
        /// <code>
        /// Default values ignored
        /// <br/> new <see cref="SqlKata.Extensions.Query{TFrom}"/>().Select()
        /// <br/>.Where(new <see cref="TFrom"/> { FirstName = "Jim", Age = 0}, includeDefaults = false)
        /// <br/> => new {FirstName = "Jim" }
        ///<para>
        /// Default values for value types includes
        /// <br/> new <see cref="SqlKata.Extensions.Query{TFrom}"/>().Select()
        /// <br/>.Where(new <see cref="TFrom"/> { FirstName = "Jim", Age = 0}, includeDefaults = false)
        /// <br/> => new {FirstName = "Jim", "Age" = 0}
        /// </para>
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <param name="query"></param>
        /// <param name="filter"></param>
        /// <param name="includeDefaults">Flag indicating whether default values for value types should be treated as set. Defaults to false</param>
        /// <returns></returns>
        /// 
        public static Query<TFrom> Where<TFrom>(this Query<TFrom> query, TFrom filter, bool includeDefaults = false)
        {
            var assignedMembers = filter.GetType().GetSelectProperties()
                                        .Select(p => (property: p, value: p.GetValue(filter)))
                                        .Where(tuple =>
                                        {
                                            var (property, value) = tuple;
                                            var propertyType = property.PropertyType;
                                            return includeDefaults 
                                                ? !(value is null) 
                                                : propertyType.IsAssigned(value);
                                        });

            foreach (var (property, value) in assignedMembers)
            {
                query.Where(property.Name, Operator.Eq, value);
            }

            return query;
        }

        public static Query<T> Where<T>(this Query<T> query, Expression<Func<T, object>> whereMember, Operator op, object value)
        {
            var whereColumn = GetColumn(whereMember);
            return query.Where(whereColumn, op, value);
        }
    }
}

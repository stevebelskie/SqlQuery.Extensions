using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SqlKata.Compilers;

namespace SqlKata.Extensions
{
    public static class Extensions
    {
        private static string GetColumn<T>(Expression<Func<T, object>> expr)
        {
            return expr switch
            {
                LambdaExpression l => l.Body switch
                {
                    MemberExpression m => m.Member.Name,
                    UnaryExpression { Operand: MemberExpression m } => m.Member.Name,
                    _ => throw new ArgumentOutOfRangeException($"The provided selector {expr} was invalid.")
                },
                _ => throw new ArgumentOutOfRangeException($"The provided selector {expr} was invalid.")
            };
        }

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

        public static Query<T> Select<T>(this Query<T> query, params Expression<Func<T, object>>[] columnSelectors)
        {
            var columns = columnSelectors.Select(GetColumn)
                                         .ToArray();

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
        /// This means <see><cref>default(T)</cref></see> (e.g. 0 for <see cref="System.Int32"/>
        /// is ignored where T is a value type, in addition to null values for reference types.
        /// </para>
        /// 
        /// <example>
        /// 
        /// <code>
        /// Default values ignored
        /// <br/> new <see cref="Query{TFrom}"/>().Select()
        /// <br/>.Where(new <see cref="TFrom"/> { FirstName = "Jim", Age = 0}, includeDefaults = false)
        /// <br/> => new {FirstName = "Jim" }
        ///<para>
        /// Default values for value types includes
        /// <br/> new <see cref="Query{TFrom}"/>().Select()
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
                query.Where(property.Name, value);
            }

            return query;
        }

        public static Query<T> Where<T>(
            this Query<T> query,
            Expression<Func<T, object>> whereMember,
            string op
        )
        {
            return null;
        }
    }
}
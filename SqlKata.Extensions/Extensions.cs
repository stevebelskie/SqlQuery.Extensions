using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SqlKata.Extensions
{
    public static class Extensions
    {
        
        public static Query Select<T>(this Query query, params Expression<Func<T, object>>[] columnSelectors)
        {
            static string GetColumn(Expression<Func<T, object>> expr)
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

            var columns = columnSelectors
                          .Select(GetColumn)
                          .Where(c => !string.IsNullOrEmpty(c))
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
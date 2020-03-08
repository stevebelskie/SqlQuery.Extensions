using System;
using System.Linq.Expressions;

namespace SqlKata.Extensions
{
    public static class Base
    {
        public static string GetColumn<T>(Expression<Func<T, object>> expr)
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
    }
}

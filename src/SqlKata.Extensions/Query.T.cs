using System;
using System.Linq;
using System.Linq.Expressions;

namespace SqlKata.Extensions
{
    public class Query<TFrom> : Query
    {
        public Query() : base(typeof(TFrom).Name)
        {

        }

        public new Query<TFrom> Select(params string[] columns)
        {
            if (!columns.Any())
            {
                return this.Select<TFrom>();
            }

            Method = "select";

            foreach (var column in columns)
            {
                AddComponent("select", new Column
                {
                    Name = column
                });
            }

            return this;
        }

        public Query<TFrom> Select(Expression<Func<TFrom, object>> whereMember, Operator op, object value)
        {
            return this.Where(whereMember, op, value);
        }
        
    }
}

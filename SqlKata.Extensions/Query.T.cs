using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

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
    }
}

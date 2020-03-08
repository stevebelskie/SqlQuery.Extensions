using System;
using System.Linq;
using System.Linq.Expressions;

namespace SqlKata.Extensions
{
    public class Query<TFrom>
    {
        private Query _sqlKataQuery;
        public Query()
        {
            _sqlKataQuery = new Query(typeof(TFrom).Name);
        }

        public static implicit operator Query(Query<TFrom> genericQuery)
        {
            return genericQuery._sqlKataQuery;
        }

        public Query<TFrom> Select(params string[] columns)
        {
            if (!columns.Any())
            {
                return this.Select<TFrom>();
            }

            Method = "select";

            foreach (var column in columns)
            {
                AddColumn(column);
            }
            return this;
        }

        public Query<TFrom> Where(string name, Operator op, object value)
        {
            AddWhere(name, op.Symbol, value);
            return this;
        }

        private string Method
        {
            set => _sqlKataQuery.Method = value;
        }

        private void AddColumn(string name)
        {
            _sqlKataQuery.AddComponent("select", new Column {Name = name});
        }

        private void AddWhere(string columnName, string op, object value)
        {
            _sqlKataQuery.Where(columnName, op, value);
        }
    }
}

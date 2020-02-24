using System;
using System.Collections.Generic;
using System.Text;

namespace SqlKata.Extensions
{
    public class Operator
    {
        public static Operator Eq => new Operator(nameof(Eq), "=");
        public static Operator Add => new Operator(nameof(Add), "+");
        public static Operator NotEqual => new Operator(nameof(NotEqual), "<>");
        
        public string Name { get; }
        public string Symbol { get; }

        private Operator(string name, string symbol)
        {
            Name = name;
            Symbol = symbol;
        }
    }
}

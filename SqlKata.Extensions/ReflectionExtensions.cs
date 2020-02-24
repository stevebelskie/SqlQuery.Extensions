using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace SqlKata.Extensions
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<T> WhereNot<T>(this IEnumerable<T> enumerable, Func<T, bool> condition)
        {
            return enumerable.Where(item => !condition(item));
        }

        public static IEnumerable<PropertyInfo> GetSelectProperties(this Type type)
        {
            return type.GetProperties()
                       .WhereNot(p => p.HasCustomAttribute<NotMappedAttribute>() ||
                              !p.HasPublicSetter());
        }

        private static bool HasPublicSetter(this PropertyInfo property) 
            => property.GetSetMethod() != null;

        
        private static bool HasCustomAttribute<TAttribute>(this MemberInfo type)
            where TAttribute : Attribute
        {
            return !(type.GetCustomAttribute<TAttribute>() is null);
        }
    }
}
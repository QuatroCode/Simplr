using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Simplr.Core.Avro
{
    public static class Helpers
    {
        public static bool CanContainNull(this Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            return !type.IsValueType || underlyingType != null;
        }
        public static IEnumerable<FieldInfo> GetAllFields(this Type t)
        {
            if (t == null)
            {
                return Enumerable.Empty<FieldInfo>();
            }

            const BindingFlags Flags =
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.DeclaredOnly;
            return t
                .GetFields(Flags)
                .Where(f => !f.IsDefined(typeof(CompilerGeneratedAttribute), false))
                .Concat(GetAllFields(t.BaseType));
        }

        public static IEnumerable<PropertyInfo> GetAllProperties(this Type t)
        {
            if (t == null)
            {
                return Enumerable.Empty<PropertyInfo>();
            }

            const BindingFlags Flags =
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.DeclaredOnly;

            return t
                .GetProperties(Flags)
                .Where(p => !p.IsDefined(typeof(CompilerGeneratedAttribute), false)
                            && p.GetIndexParameters().Length == 0)
                .Concat(GetAllProperties(t.BaseType));
        }

        public static IList<PropertyInfo> RemoveDuplicates(IEnumerable<PropertyInfo> properties)
        {
            var result = new List<PropertyInfo>();
            foreach (var p in properties)
            {
                if (result.Find(s => s.Name == p.Name) == null)
                {
                    result.Add(p);
                }
            }

            return result;
        }
    }
}

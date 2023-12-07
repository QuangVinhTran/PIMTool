using System;
using System.Collections.Generic;
using System.Linq;

namespace PIMTool.Core.Helpers;

public static class ReflectionHelper
{
    public static IEnumerable<Type> GetClassesFromAssignableType(Type targetType)
    {
        var types = (from asm in AppDomain.CurrentDomain.GetAssemblies()
            from type in asm.GetTypes()
            where type.IsClass && !type.IsAbstract && type.IsAssignableTo(targetType)
            select type);
        return types;
    }

    public static object GetPropertyValueByName<T>(T obj, string propertyName)
    {
        if (obj is null || obj.Equals(default) || propertyName.IsNullOrEmpty())
        {
            return "";
        }
        
        var types = (from asm in AppDomain.CurrentDomain.GetAssemblies()
            from type in asm.GetTypes()
            where type.FullName == typeof(T).FullName
            select type);

        try
        {
            var t = types.First();
            var property = t.GetProperties()
                .First(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));

            return property.GetValue(obj) ?? "";
        }
        catch
        {
            return "";
        }
    }
}
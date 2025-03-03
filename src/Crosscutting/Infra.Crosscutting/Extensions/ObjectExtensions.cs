using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Goal.Infra.Crosscutting.Extensions;

public static class ObjectExtensions
{
    public static IDictionary<string, object?> ToDictionary(this object source)
    {
        var dictionary = new Dictionary<string, object?>();

        if (source is not null)
        {
            IEnumerable<PropertyInfo> properties = source.GetType().GetTypeInfo().DeclaredProperties;

            object? value;
            foreach (PropertyInfo property in properties)
            {
                value = property.GetValue(source);
                dictionary.Add(property.Name, value);
            }
        }

        return dictionary;
    }

    public static IDictionary<string, TValue?> ToDictionary<TValue>(this object source)
    {
        if (source is not null)
        {
            IEnumerable<PropertyInfo> properties = source.GetType().GetTypeInfo().DeclaredProperties;
            IEnumerable<KeyValuePair<string, TValue?>> values = properties.Select(p =>
            {
                return new KeyValuePair<string, TValue?>(
                    p.Name,
                    p.GetValue(source).ConvertTo(default(TValue))
                );
            });

            return new Dictionary<string, TValue?>(values);
        }

        return new Dictionary<string, TValue?>();
    }

    public static TType? ConvertTo<TType>(this object? value)
        => (TType?)Convert.ChangeType(value, typeof(TType));

    public static TType? ConvertTo<TType>(this object? value, TType? defaultValue)
    {
        try
        {
            return value.ConvertTo<TType>();
        }
        catch
        {
            return defaultValue;
        }
    }

    public static TAttribute? GetAttribute<TAttribute>(this object source)
        where TAttribute : Attribute
    {
        Attribute? attr = source.GetType().GetCustomAttribute<TAttribute>(false);

        return attr is null
            ? null
            : (TAttribute?)attr;
    }

    public static bool IsDefined<TAttribute>(this object source)
        where TAttribute : Attribute
        => source.GetAttribute<TAttribute>() != null;
}

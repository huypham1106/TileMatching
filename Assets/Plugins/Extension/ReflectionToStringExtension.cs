using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System;

public static class ReflectionToStringExtension
{
    public static bool verbose = false;
/*    public static string ToStringExtCollection<T>(this ICollection<T> obj, int d = 0)
    {
        return (verbose ? "Collection<" + typeof(T).Name + ">" : "") + "{" + string.Join(", ", obj.Select(o => o.ToStringExt(d + 1))) + "}";
    }
    public static string ToStringExtKeyValuePair<U, V>(this KeyValuePair<U, V> kvp, int d = 0)
    {
        return (verbose ? "KeyValuePair<" + typeof(U).Name + "," + typeof(V).Name + ">" : "") +
            kvp.Key.ToStringExt(d + 1) +
            " => " +
            kvp.Value.ToStringExt(d + 1);
    }*/
    /*public static string ToStringExt<T>(this T obj, int d = 0)
    {
        string res = null;
        Type TType = typeof(T);
        if (TType.IsGenericType
            && (TType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>)))
        {
            Type kvpType = typeof(KeyValuePair<,>).MakeGenericType(TType.GetGenericArguments()[0], TType.GetGenericArguments()[1]);
            res = ToStringExtKeyValuePair(obj.CastToReflected(kvpType), d + 1);
        }
        else
        {
            foreach (var IType in TType.GetInterfaces())
            {
                if (IType.IsGenericType
                    && (IType.GetGenericTypeDefinition() == typeof(ICollection<>)))
                {
                    res = ToStringExtCollection(obj.CastToReflected(IType), d + 1);
                    break;
                }
            }
        }
        if (res == null)
        {
            res = obj.ToString();
        }
        return res;
    }*/

    public static T CastTo<T>(this object o) => (T)o;

    public static dynamic CastToReflected(this object o, Type type)
    {
        var methodInfo = typeof(ReflectionToStringExtension).GetMethod(nameof(CastTo), BindingFlags.Static | BindingFlags.Public);
        var genericArguments = new[] { type };
        var genericMethodInfo = methodInfo?.MakeGenericMethod(genericArguments);
        return genericMethodInfo?.Invoke(null, new[] { o });
    }
    public static string Replace(this string source, string oldValue, string newValue, StringComparison comparisonType)
    {
        if (source.Length == 0 || oldValue.Length == 0)
            return source;

        var result = new System.Text.StringBuilder();
        int startingPos = 0;
        int nextMatch;
        while ((nextMatch = source.IndexOf(oldValue, startingPos, comparisonType)) > -1)
        {
            result.Append(source, startingPos, nextMatch - startingPos);
            result.Append(newValue);
            startingPos = nextMatch + oldValue.Length;
        }
        result.Append(source, startingPos, source.Length - startingPos);

        return result.ToString();
    }
} 

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Marten.Linq.Fields;
using NpgsqlTypes;
using Weasel.Postgresql.SqlGeneration;

namespace Marten.Linq.Parsing;

public class DictionaryExpressions: IMethodCallParser
{
    public bool Matches(MethodCallExpression expression)
    {
        return IsCollectionContainsWithStringKey(expression.Method)
               || IsDictionaryContainsKey(expression.Method);
    }

    public ISqlFragment Parse(IFieldMapping mapping, IReadOnlyStoreOptions options, MethodCallExpression expression)
    {
        var fieldlocator = mapping.FieldFor(expression).TypedLocator;

        if (IsCollectionContainsWithStringKey(expression.Method))
        {
            return QueryFromICollectionContains(expression, fieldlocator, options.Serializer());
        }

        if (IsDictionaryContainsKey(expression.Method))
        {
            return QueryFromDictionaryContainsKey(expression, fieldlocator);
        }

        throw new InvalidOperationException("Could not understand the format of the dictionary access");
    }

    private static bool IsCollectionContainsWithStringKey(MethodInfo m)
    {
        return m.Name == nameof(IDictionary<string, string>.Contains)
               && m.DeclaringType != null && m.DeclaringType.IsConstructedGenericType
               && m.DeclaringType.GetGenericTypeDefinition() == typeof(ICollection<>)
               && m.DeclaringType.GenericTypeArguments[0].IsConstructedGenericType
               && m.DeclaringType.GenericTypeArguments[0].GetGenericTypeDefinition() == typeof(KeyValuePair<,>)
               && m.DeclaringType.GenericTypeArguments[0].GenericTypeArguments[0] == typeof(string);
    }

    private static bool IsDictionaryContainsKey(MethodInfo m)
    {
        return m.Name == nameof(IDictionary<string, string>.ContainsKey)
               && m.DeclaringType != null && m.DeclaringType.IsConstructedGenericType
               && m.DeclaringType.GetGenericTypeDefinition() == typeof(IDictionary<,>)
               && m.DeclaringType.GenericTypeArguments[0] == typeof(string);
    }

    private static ISqlFragment QueryFromDictionaryContainsKey(MethodCallExpression expression, string fieldLocator)
    {
        var key = (string)expression.Arguments[0].Value();
        // have to use different token here because we actually want the `?` character as the operator!
        return new CustomizableWhereFragment($"{fieldLocator} ? @1", "@1",
            new CommandParameter(key, NpgsqlDbType.Text));
    }

    private static ISqlFragment QueryFromICollectionContains(MethodCallExpression expression, string fieldPath,
        ISerializer serializer)
    {
        var constant = (ConstantExpression)expression.Arguments[0];
        var kvp = constant.Value; // is kvp<string, unknown>
        var kvpType = kvp.GetType();
        var key = kvpType.GetProperty("Key").GetValue(kvp);
        var value = kvpType.GetProperty("Value").GetValue(kvp);
        var dictType =
            typeof(Dictionary<,>).MakeGenericType(kvpType.GenericTypeArguments[0], kvpType.GenericTypeArguments[1]);
        var dict = dictType.GetConstructors()[0].Invoke(null);
        dictType.GetMethod("Add").Invoke(dict, new[] { key, value });
        var json = serializer.ToJson(dict);
        return new CustomizableWhereFragment($"{fieldPath} @> ?", "?", new CommandParameter(json, NpgsqlDbType.Jsonb));
    }
}

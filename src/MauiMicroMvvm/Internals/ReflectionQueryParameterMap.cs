using System.Reflection;

namespace MauiMicroMvvm.Internals;

internal static class ReflectionQueryParameterMap
{
    public static IQueryParameterMap Create(Type viewModelType)
    {
        var setters = viewModelType
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => property.CanWrite)
            .Select(property => new ReflectionQueryParameterSetter(property));

        return new QueryParameterMap(setters);
    }
}

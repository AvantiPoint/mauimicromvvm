using System.Globalization;
using System.Reflection;
using System.Text.Json;

namespace MauiMicroMvvm.Internals;

internal sealed class ReflectionQueryParameterSetter : IQueryParameterSetter
{
    private readonly PropertyInfo _propertyInfo;

    public ReflectionQueryParameterSetter(PropertyInfo propertyInfo)
    {
        ArgumentNullException.ThrowIfNull(propertyInfo);

        if (!propertyInfo.CanWrite)
            throw new ArgumentException("Query parameter properties must be writable.", nameof(propertyInfo));

        _propertyInfo = propertyInfo;
        Name = propertyInfo.Name;
        ParameterType = propertyInfo.PropertyType;
    }

    public string Name { get; }

    public Type ParameterType { get; }

    public bool TrySet(object target, object? value)
    {
        _propertyInfo.SetValue(target, ConvertValue(value));
        return true;
    }

    private object? ConvertValue(object? value)
    {
        if (value is null)
            return null;

        var conversionType = Nullable.GetUnderlyingType(ParameterType) ?? ParameterType;
        if (conversionType.IsInstanceOfType(value))
            return value;

        if (conversionType == typeof(string))
            return Convert.ToString(value, CultureInfo.InvariantCulture);

        if (conversionType.IsEnum)
        {
            return value is string enumValue
                ? Enum.Parse(conversionType, enumValue)
                : Enum.ToObject(conversionType, value);
        }

        if (typeof(IConvertible).IsAssignableFrom(conversionType) && value is IConvertible)
            return Convert.ChangeType(value, conversionType, CultureInfo.InvariantCulture);

        if (value is string json)
            return JsonSerializer.Deserialize(json, conversionType);

        if (value is JsonElement jsonElement)
            return jsonElement.Deserialize(conversionType);

        var serializedValue = JsonSerializer.Serialize(value);
        return JsonSerializer.Deserialize(serializedValue, conversionType);
    }
}

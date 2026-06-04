using MauiMicroMvvm;

namespace MauiMicroMvvm.Tests.Mocks;

internal sealed class TestQueryParameterSetter : IQueryParameterSetter
{
    private readonly Action<object, object?> _setValue;

    public TestQueryParameterSetter(string name, Type parameterType, Action<object, object?> setValue)
    {
        Name = name;
        ParameterType = parameterType;
        _setValue = setValue;
    }

    public string Name { get; }

    public Type ParameterType { get; }

    public bool TrySet(object target, object? value)
    {
        _setValue(target, value);
        return true;
    }
}

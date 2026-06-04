using MauiMicroMvvm;

namespace MauiMicroMvvm.Tests.Mocks;

internal sealed class TestQueryParameterSetter : IQueryParameterSetter
{
    private readonly Func<object, object?, bool> _setValue;

    public TestQueryParameterSetter(string name, Type parameterType, Action<object, object?> setValue)
        : this(name, parameterType, (target, value) =>
        {
            setValue(target, value);
            return true;
        })
    {
    }

    public TestQueryParameterSetter(string name, Type parameterType, Func<object, object?, bool> setValue)
    {
        Name = name;
        ParameterType = parameterType;
        _setValue = setValue;
    }

    public string Name { get; }

    public Type ParameterType { get; }

    public bool TrySet(object target, object? value)
    {
        return _setValue(target, value);
    }
}

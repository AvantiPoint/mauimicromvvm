namespace MauiMicroMvvm;

public interface IQueryParameterSetter
{
    string Name { get; }

    Type ParameterType { get; }

    bool TrySet(object target, object? value);
}

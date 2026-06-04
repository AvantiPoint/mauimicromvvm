namespace MauiMicroMvvm;

public interface IQueryParameterMap
{
    bool TryGetSetter(string key, out IQueryParameterSetter setter);

    bool TrySet(object target, string key, object? value);
}

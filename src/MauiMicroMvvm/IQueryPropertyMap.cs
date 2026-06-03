namespace MauiMicroMvvm;

public interface IQueryPropertyMap
{
    bool TryGetProperty(string key, out IQueryProperty property);
}

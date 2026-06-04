namespace MauiMicroMvvm;

public class QuerystringPropertyException : Exception
{
    public QuerystringPropertyException(string key, Exception innerException)
        : base($"Failed to set {key}", innerException)
    {
        Property = key;
    }

    public string Property { get; }
}

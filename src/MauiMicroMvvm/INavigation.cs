namespace MauiMicroMvvm;

public interface INavigation
{
    Task GoToAsync(string uri);
    Task GoToAsync(string uri, IDictionary<string, object> parameters);
    Task GoToAsync(string uri, bool animate);
    Task GoToAsync(string uri, bool animate, IDictionary<string, object> parameters);
}

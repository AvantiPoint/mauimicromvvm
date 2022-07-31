using System.ComponentModel;

namespace MauiMicroMvvm.Internals;

[EditorBrowsable(EditorBrowsableState.Never)]
public class DefaultNavigation : INavigation
{
    private readonly Shell _shell;

    public DefaultNavigation(Shell shell)
    {
        _shell = shell;
    }

    public Task GoToAsync(string uri) => 
        _shell.GoToAsync(uri);

    public Task GoToAsync(string uri, IDictionary<string, object> parameters) => 
        _shell.GoToAsync(uri, parameters);

    public Task GoToAsync(string uri, bool animate) => 
        _shell.GoToAsync(uri, animate);

    public Task GoToAsync(string uri, bool animate, IDictionary<string, object> parameters) => 
        _shell.GoToAsync(uri, animate, parameters);
}

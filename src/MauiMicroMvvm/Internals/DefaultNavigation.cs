using System.ComponentModel;

namespace MauiMicroMvvm.Internals;

[EditorBrowsable(EditorBrowsableState.Never)]
public class DefaultNavigation<TShell> : INavigation where TShell : Shell
{
    private readonly TShell _shell;

    public DefaultNavigation(TShell shell)
    {
        _shell = shell;
    }

    public async Task GoToAsync(string uri) =>
        await _shell.GoToAsync(uri);

    public async Task GoToAsync(string uri, IDictionary<string, object> parameters) =>
        await _shell.GoToAsync(uri, parameters);

    public async Task GoToAsync(string uri, bool animate) =>
        await _shell.GoToAsync(uri, animate);

    public async Task GoToAsync(string uri, bool animate, IDictionary<string, object> parameters) =>
        await _shell.GoToAsync(uri, animate, parameters);
}

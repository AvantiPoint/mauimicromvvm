using System.ComponentModel;

namespace MauiMicroMvvm.Internals;

[EditorBrowsable(EditorBrowsableState.Never)]
public class DefaultNavigation<TShell> : INavigation where TShell : Shell
{
    private readonly Lazy<TShell> _lazyShell;
    private TShell Shell => _lazyShell.Value;

    public DefaultNavigation(IServiceProvider services)
    {
        _lazyShell = new Lazy<TShell>(services.GetRequiredService<TShell>);
    }

    public async Task GoToAsync(string uri) =>
        await Shell.GoToAsync(uri);

    public async Task GoToAsync(string uri, IDictionary<string, object> parameters) =>
        await Shell.GoToAsync(uri, parameters);

    public async Task GoToAsync(string uri, bool animate) =>
        await Shell.GoToAsync(uri, animate);

    public async Task GoToAsync(string uri, bool animate, IDictionary<string, object> parameters) =>
        await Shell.GoToAsync(uri, animate, parameters);
}

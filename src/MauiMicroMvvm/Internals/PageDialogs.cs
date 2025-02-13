namespace MauiMicroMvvm.Internals;

internal class PageDialogs<TShell> : IPageDialogs where TShell : Shell
{
    private readonly Lazy<TShell> _lazyShell;
    private TShell Shell => _lazyShell.Value;

    public PageDialogs(IServiceProvider services)
    {
        _lazyShell = new Lazy<TShell>(services.GetRequiredService<TShell>);
    }

    public Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons) =>
        Shell.DisplayActionSheet(title, cancel, destruction, buttons);

    public Task<string> DisplayActionSheet(string title, string cancel, string destruction, FlowDirection flowDirection, params string[] buttons) =>
        Shell.DisplayActionSheet(title, cancel, destruction, flowDirection, buttons);

    public Task DisplayAlert(string title, string message, string cancel) =>
        Shell.DisplayAlert(title, message, cancel);

    public Task DisplayAlert(string title, string message, string cancel, FlowDirection flowDirection) =>
        Shell.DisplayAlert(title, message, cancel, flowDirection);

    public Task<bool> DisplayAlert(string title, string message, string accept, string cancel) =>
        Shell.DisplayAlert(title, message, accept, cancel);

    public Task<bool> DisplayAlert(string title, string message, string accept, string cancel, FlowDirection flowDirection) =>
        Shell.DisplayAlert(title, message, accept, cancel, flowDirection);
}

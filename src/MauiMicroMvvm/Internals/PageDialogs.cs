namespace MauiMicroMvvm.Internals;

internal class PageDialogs<TShell> : IPageDialogs where TShell : Shell
{
    private readonly TShell _shell;

    public PageDialogs(TShell shell)
    {
        _shell = shell;
    }

    public Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons) =>
        _shell.DisplayActionSheet(title, cancel, destruction, buttons);

    public Task<string> DisplayActionSheet(string title, string cancel, string destruction, FlowDirection flowDirection, params string[] buttons) =>
        _shell.DisplayActionSheet(title, cancel, destruction, flowDirection, buttons);

    public Task DisplayAlert(string title, string message, string cancel) =>
        _shell.DisplayAlert(title, message, cancel);

    public Task DisplayAlert(string title, string message, string cancel, FlowDirection flowDirection) =>
        _shell.DisplayAlert(title, message, cancel, flowDirection);

    public Task<bool> DisplayAlert(string title, string message, string accept, string cancel) =>
        _shell.DisplayAlert(title, message, accept, cancel);

    public Task<bool> DisplayAlert(string title, string message, string accept, string cancel, FlowDirection flowDirection) =>
        _shell.DisplayAlert(title, message, accept, cancel, flowDirection);
}

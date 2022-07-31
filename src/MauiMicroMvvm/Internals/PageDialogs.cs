namespace MauiMicroMvvm.Internals;

internal class PageDialogs : IPageDialogs
{
    private readonly Shell _shell;

    public PageDialogs(Shell shell)
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

    public Task DisplayAlert(string title, string message, string accept, string cancel) =>
        _shell.DisplayAlert(title, message, accept, cancel);

    public Task DisplayAlert(string title, string message, string accept, string cancel, FlowDirection flowDirection) =>
        _shell.DisplayAlert(title, message, accept, cancel, flowDirection);
}

namespace MauiMicroMvvm;

public interface IPageDialogs
{
    Task DisplayAlert(string title, string message, string cancel);

    Task DisplayAlert(string title, string message, string cancel, FlowDirection flowDirection);

    Task DisplayAlert(string title, string message, string accept, string cancel);

    Task DisplayAlert(string title, string message, string accept, string cancel, FlowDirection flowDirection);

    Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons);

    Task<string> DisplayActionSheet(string title, string cancel, string destruction, FlowDirection flowDirection, params string[] buttons);
}

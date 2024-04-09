namespace MauiMicroMvvm.Internals;

internal class WindowCreator<TShell>(TShell Shell) : IWindowCreator
    where TShell : Shell
{
    private Window? _window;

    public Window CreateWindow(Application app, IActivationState? activationState)
    {
        return _window ??= new Window
        {
            Page = Shell
        };
    }
}

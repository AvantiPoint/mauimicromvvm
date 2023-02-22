using System.Windows.Input;
using MauiMicroMvvm;

namespace MauiMicroApp._1.ViewModels;

public class MainViewModel : MauiMicroViewModel
{
    private ISemanticScreenReader _screenReader { get; }
    private int _count;

    public MainViewModel(ViewModelContext context, ISemanticScreenReader screenReader)
        : base(context)
    {
        _screenReader = screenReader;
        ClickCommand = new Command(OnClickCommand);
    }

    public string Message
    {
        get => Get<string>("Click Me");
        set => Set(value);
    }

    public ICommand ClickCommand { get; }

    private void OnClickCommand()
    {
        if (++_count == 1)
            Message = "Pressed once!";
        else
            Message = $"Pressed {_count} times!";

        _screenReader.Announce(Message);
    }
}

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
#if Reactive
        Message = "Click Me";
        ClickCommand = ReactiveCommand.Create(OnClickCommand);
#else
        ClickCommand = new Command(OnClickCommand);
#endif
    }

#if Reactive
    [Reactive]
    public string Message { get; set; }
#else
    public string Message
    {
        get => Get<string>("Click Me");
        set => Set(value);
    }
#endif

    public ICommand ClickCommand { get; }

    private void OnClickCommand()
    {
        if (++_count == 1)
            Message = "Pressed once!";
        else
            Message = $"Pressed {_count} times!";

        _screenReader.Announce(Message);

        if (_count > 3)
            PageDialogs.DisplayAlert("Stop it!", "You're pressing too much!", "OK");
    }
}

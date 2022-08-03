using MauiMicroMvvm;
using ReactiveUI;

namespace MauiMicroSample.ViewModels;

public class MessageDisplayViewModel : RxMauiMicroViewModel
{
    public MessageDisplayViewModel(ViewModelContext context) : base(context)
    {
    }

    private string _message;
    public string Message
    {
        get => _message;
        set => this.RaiseAndSetIfChanged(ref _message, value);
    }
}

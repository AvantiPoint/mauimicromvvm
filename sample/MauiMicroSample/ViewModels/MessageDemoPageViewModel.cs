using MauiMicroMvvm;
using ReactiveUI;
using Microsoft.Extensions.Logging;
using System.Reactive.Disposables;

namespace MauiMicroSample.ViewModels;

public class MessageDemoPageViewModel : RxMauiMicroViewModel
{
    public MessageDemoPageViewModel(ViewModelContext context) 
        : base(context)
    {
        Disposables.Add(AppLifecycle.Subscribe(state => Logger.LogInformation($"Application Lifecycle State: {state}")));
        Disposables.Add(ViewLifecycle.Subscribe(state => Logger.LogInformation($"View Lifecycle State: {state}")));
    }

    private string _message = string.Empty;
    public string Message
    {
        get => _message;
        set => this.RaiseAndSetIfChanged(ref _message, value);
    }
}

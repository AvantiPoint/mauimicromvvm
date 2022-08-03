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
        AppLifecycle.Subscribe(state => Logger.LogInformation($"Application Lifecycle State: {state}"))
            .DisposeWith(Disposables);
        ViewLifecycle.Subscribe(state => Logger.LogInformation($"View Lifecycle State: {state}"))
            .DisposeWith(Disposables);
    }

    private string _message;
    public string Message
    {
        get => _message;
        set => this.RaiseAndSetIfChanged(ref _message, value);
    }
}

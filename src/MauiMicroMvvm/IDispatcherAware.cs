namespace MauiMicroMvvm;

#nullable enable
public interface IDispatcherAware
{
    IDispatcher? Dispatcher { get; set; }
}

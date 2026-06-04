using MauiMicroMvvm;
using ReactiveUI;
using System.Reactive.Disposables;

namespace MauiMicroMvvm.Rx.Tests.Mocks;

internal class TestRxViewModel : RxMauiMicroViewModel
{
    public TestRxViewModel(ViewModelContext context) : base(context)
    {
    }

    public new IObservable<ViewLifecycleState> ViewLifecycle => base.ViewLifecycle;
    public new IObservable<AppLifecycleState> AppLifecycle => base.AppLifecycle;
    public new IObservable<IDictionary<string, object>> OnParametersSet => base.OnParametersSet;
    public new CompositeDisposable Disposables => base.Disposables;
}

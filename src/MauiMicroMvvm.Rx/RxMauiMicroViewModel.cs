using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace MauiMicroMvvm;

public class RxMauiMicroViewModel : ReactiveObject, IViewModelActivation, IViewLifecycle, IAppLifecycle, IQueryAttributable, IDisposable
{
    private readonly Subject<AppLifecycleState> _applifecycleState;
    private readonly Subject<ViewLifecycleState> _viewLifecycleState;
    private readonly Subject<IDictionary<string, object>> _queryParameters;
    private readonly Lazy<ILogger> _lazyLogger;
    protected ObservableAsPropertyHelper<bool> IsBusyHelper;
    private readonly ObservableAsPropertyHelper<bool> _isNotBusyHelper;
    protected readonly CompositeDisposable Disposables;

    public RxMauiMicroViewModel(ViewModelContext context)
    {
        _applifecycleState = new Subject<AppLifecycleState>();
        _viewLifecycleState = new Subject<ViewLifecycleState>();
        _queryParameters = new Subject<IDictionary<string, object>>();
        Disposables = new CompositeDisposable();
        Navigation = context.Navigation;
        PageDialogs = context.PageDialogs;
        _lazyLogger = new Lazy<ILogger>(() => context.Logger.CreateLogger(GetType().Name));
        this.WhenAnyValue(x => x.IsBusy)
            .Select(x => !x)
            .ToProperty(this, nameof(IsNotBusy), out _isNotBusyHelper, () => true)
            .DisposeWith(Disposables);
    }

    private bool _isDisposed;
    public bool IsDisposed
    {
        get => _isDisposed;
        private set => this.RaiseAndSetIfChanged(ref _isDisposed, value);
    }

    public bool IsBusy => IsBusyHelper?.Value ?? false;

    public bool IsNotBusy => _isNotBusyHelper.Value;

    protected ILogger Logger => _lazyLogger.Value;

    protected INavigation Navigation { get; }

    protected IPageDialogs PageDialogs { get; }

    protected IObservable<ViewLifecycleState> ViewLifecycle => _viewLifecycleState;

    protected IObservable<AppLifecycleState> AppLifecycle => _applifecycleState;

    protected IObservable<IDictionary<string, object>> OnParametersSet => _queryParameters;

    void IViewModelActivation.OnFirstLoad() => 
        _viewLifecycleState.OnNext(ViewLifecycleState.FirstLoad);

    void IViewLifecycle.OnAppearing() => 
        _viewLifecycleState.OnNext(ViewLifecycleState.Appearing);

    void IViewLifecycle.OnDisappearing() => 
        _viewLifecycleState.OnNext(ViewLifecycleState.Appearing);

    void IAppLifecycle.OnResume() => 
        _applifecycleState.OnNext(AppLifecycleState.Resume);

    void IAppLifecycle.OnSleep() => 
        _applifecycleState.OnNext(AppLifecycleState.Sleep);

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query) => 
        _queryParameters.OnNext(query);

    protected virtual void Dispose(bool disposing) { }

    public void Dispose()
    {
        Dispose(disposing: true);
        if(!Disposables.IsDisposed)
            Disposables.Dispose();

        GC.SuppressFinalize(this);

        if (!IsDisposed)
            IsDisposed = true;
    }
}
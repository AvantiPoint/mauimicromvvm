#nullable enable
namespace MauiMicroMvvm.Internals;

internal class ViewFactory : IViewFactory
{
    internal static readonly BindableProperty NavigationKeyProperty =
        BindableProperty.CreateAttached("NavigationKey", typeof(string), typeof(ViewFactory), null);

    internal static string? GetNavigationKey(BindableObject bindable) =>
        (string?)bindable.GetValue(NavigationKeyProperty);

    internal static void SetNavigationKey(BindableObject bindable, string? value) =>
        bindable.SetValue(NavigationKeyProperty, value);

    private IServiceProvider _services { get; }
    private IEnumerable<ViewMapping> _mappings { get; }

    public ViewFactory(IServiceProvider services, IEnumerable<ViewMapping> mappings)
    {
        _services = services;
        _mappings = mappings;
    }

    public TView CreateView<TView>()
        where TView : VisualElement
    {
        var view = ActivatorUtilities.CreateInstance<TView>(_services);
        Configure(view);
        return view;
    }

    public VisualElement CreateView(string key)
    {
        var mapping = _mappings.FirstOrDefault(x => x.Name == key);
        if(mapping is null)
            throw new KeyNotFoundException(key);

        var view = (VisualElement)ActivatorUtilities.CreateInstance(_services, mapping.View);
        SetNavigationKey(view, key);
        Configure(view);
        return view;
    }

    public TView Configure<TView>(TView view)
        where TView : VisualElement
    {
        if(view.BindingContext is null && (!view.IsSet(Xaml.MauiMicro.AutowireProperty) || Xaml.MauiMicro.GetAutowire(view)))
            SetBindingContext(view);

        if (view is Shell || view is Window || view.Behaviors.OfType<AppLifecycleBehavior>().Any())
            return view;

        else if (view is Page page)
            page.Behaviors.Add(new AppLifecycleBehavior { Page = page, View = view });

        else
        {
            void OnParentSet(object? sender, EventArgs args)
            {
                // This shouldn't ever happen, but provides cleaner approach
                if (sender is not Element element)
                    return;

                element.ParentChanged -= OnParentSet;
                if (element.Parent is Page page)
                    page.Behaviors.Add(new AppLifecycleBehavior { Page = page, View = view });
                else if (element.Parent is not null)
                {
                    var root = element.Parent;
                    while (true)
                    {
                        if (root.Parent is null)
                        {
                            root.ParentChanged += OnParentSet;
                            break;
                        }

                        root = root.Parent;
                    }
                }
            }

            if (view is Element element)
                element.ParentChanged += OnParentSet;
        }

        return view;
    }

    private void SetBindingContext(VisualElement view)
    {
        var mapping = GetViewMapping(view);
        if (mapping?.ViewModel is null)
            return;

        view.BindingContext = _services.GetRequiredService(mapping.ViewModel);
        if (view.BindingContext is IDispatcherAware dispatcherAware)
            dispatcherAware.Dispatcher = view.Dispatcher;
    }

    private ViewMapping? GetViewMapping(VisualElement view)
    {
        var key = GetNavigationKey(view);
        return string.IsNullOrEmpty(key)
            ? _mappings.FirstOrDefault(x => x.View == view.GetType())
            : _mappings.FirstOrDefault(x => x.Name == key);
    }
}

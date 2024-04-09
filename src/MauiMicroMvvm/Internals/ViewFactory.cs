#nullable enable
using MauiMicroMvvm.Behaviors;

namespace MauiMicroMvvm.Internals;

public class ViewFactory(IServiceProvider services, IEnumerable<ViewMapping> mappings, IBehaviorFactory behaviorFactory) : IViewFactory
{
    public static readonly BindableProperty NavigationKeyProperty =
        BindableProperty.CreateAttached("NavigationKey", typeof(string), typeof(ViewFactory), null);

    public static string? GetNavigationKey(BindableObject bindable) =>
        (string?)bindable.GetValue(NavigationKeyProperty);

    public static void SetNavigationKey(BindableObject bindable, string? value) =>
        bindable.SetValue(NavigationKeyProperty, value);

    protected IServiceProvider Services { get; } = services;
    protected IEnumerable<ViewMapping> Mappings { get; } = mappings;

    public TView CreateView<TView>()
        where TView : VisualElement
    {
        var view = ActivatorUtilities.CreateInstance<TView>(Services);
        Configure(view);
        return view;
    }

    public VisualElement CreateView(string key)
    {
        var mapping = Mappings.FirstOrDefault(x => x.Name == key) ?? throw new KeyNotFoundException(key);
        var view = (VisualElement)ActivatorUtilities.CreateInstance(Services, mapping.View);
        SetNavigationKey(view, key);
        Configure(view);
        return view;
    }

    public virtual TView Configure<TView>(TView view)
        where TView : VisualElement
    {
        if(view.BindingContext is null && (!view.IsSet(Xaml.MauiMicro.AutowireProperty) || Xaml.MauiMicro.GetAutowire(view)))
            SetBindingContext(view);

        behaviorFactory.ApplyBehaviors(view);

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

        view.BindingContext = Services.GetRequiredService(mapping.ViewModel);
    }

    private ViewMapping? GetViewMapping(VisualElement view)
    {
        var key = GetNavigationKey(view);
        return string.IsNullOrEmpty(key)
            ? Mappings.FirstOrDefault(x => x.View == view.GetType())
            : Mappings.FirstOrDefault(x => x.Name == key);
    }
}

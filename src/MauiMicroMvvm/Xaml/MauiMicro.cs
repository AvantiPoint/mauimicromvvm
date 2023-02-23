using MauiMicroMvvm.Internals;

[assembly: XmlnsDefinition("http://schemas.mauimicromvvm.com/2022/dotnet/maui", "MauiMicroMvvm.Xaml")]

namespace MauiMicroMvvm.Xaml;

// All the code in this file is included in all platforms.
#nullable enable
public static class MauiMicro
{
    public static readonly BindableProperty AutowireProperty =
        BindableProperty.CreateAttached("Autowire", typeof(bool), typeof(MauiMicro), null, propertyChanged: OnMvvmChanged);

    public static bool GetAutowire(BindableObject bindable) =>
        (bool)bindable.GetValue(AutowireProperty);

    public static void SetAutowire(BindableObject bindable, bool value) => 
        bindable.SetValue(AutowireProperty, value);

    public static readonly BindableProperty SharedContextProperty =
        BindableProperty.CreateAttached("SharedContext", typeof(object), typeof(MauiMicro), null, BindingMode.TwoWay);

    public static object? GetSharedContext(BindableObject bindable) =>
        bindable.GetValue(SharedContextProperty);

    public static void SetSharedContext(BindableObject bindable, object? value) =>
        bindable.SetValue(SharedContextProperty, value);

    public static readonly BindableProperty RouteProperty =
        BindableProperty.CreateAttached("Route", typeof(string), typeof(MauiMicro), null, propertyChanged: OnRouteChanged);

    public static string? GetRoute(BindableObject bindable) =>
        (string?)bindable.GetValue(RouteProperty);

    public static void SetRoute(BindableObject bindable, string? value) =>
        bindable.SetValue(RouteProperty, value);

    private static void OnRouteChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var content = GetContent(bindable);
        if (content is null)
            return;

        var route = GetRoute(bindable);
        if (route is null)
            return;

        content.Route = route;
        SetContentTempalte(content, route);
    }

    private static void SetContentTempalte(ShellContent content, string route)
    {
        content.ContentTemplate = new DataTemplate(() => CreateView(route));
    }

    private static Page CreateView(string route)
    {
        var handler = Shell.Current.Handler;
        if (handler is null)
            throw new Exception("Handler is null");
        var factory = handler.MauiContext!.Services.GetRequiredService<IViewFactory>();
        return (Page)factory.CreateView(route);
    }

    private static ShellContent GetContent(BindableObject bindable)
    {
        if (bindable is ShellContent content)
            return content;

        else if (bindable is Tab tab)
        {
            if(tab.CurrentItem is null)
            {
                content = new ShellContent();
                tab.Items.Add(content);
                tab.CurrentItem = content;
                return content;
            }

            return tab.CurrentItem;
        }

        throw new InvalidOperationException($"The Route was used on an unsupported type '{bindable.GetType().FullName}'.");
    }

    private static void OnMvvmChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (newValue is bool autowire && autowire)
        {
            void Configure(IServiceProvider services)
            {
                var factory = services.GetRequiredService<IViewFactory>();
                if (bindable is VisualElement view)
                    factory.Configure(view);
            }

            var services = GetServices(bindable);
            if (services is not null)
                Configure(services);
            else if (bindable is VisualElement visualElement)
            {
                void OnHandlerChanged(object? sender, EventArgs e)
                {
                    visualElement.HandlerChanged -= OnHandlerChanged;
                    services = GetServices(visualElement);
                    if(services is not null)
                        Configure(services);
                }
                visualElement.HandlerChanged += OnHandlerChanged;
            }
        }
    }

    private static IServiceProvider? GetServices(BindableObject bindable)
    {
        if (bindable is null)
            return null;
        else if (bindable is Shell shell)
            return shell.Handler?.MauiContext?.Services ?? GetServices(shell.Parent);
        else if (bindable is Window window)
            return window.Handler?.MauiContext?.Services ?? Application.Current?.Handler?.MauiContext?.Services;

        return GetServices(Shell.Current);
    }
}

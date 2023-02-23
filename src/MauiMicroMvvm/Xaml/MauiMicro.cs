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

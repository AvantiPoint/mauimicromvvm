using MauiMicroMvvm.Internals;

[assembly: XmlnsDefinition("http://schemas.mauimicromvvm.com/2022/dotnet/maui", "MauiMicroMvvm.Xaml")]

namespace MauiMicroMvvm.Xaml;

// All the code in this file is included in all platforms.
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

    public static object GetSharedContext(BindableObject bindable) =>
        bindable.GetValue(SharedContextProperty);

    public static void SetSharedContext(BindableObject bindable, object value) =>
        bindable.SetValue(SharedContextProperty, value);

    private static void OnMvvmChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue is bool autowire && autowire)
        {
            var type = MvvmMapper.GetViewModelType(bindable.GetType());

            var services = GetServices(bindable);
            if(services is not null)
                bindable.BindingContext = services.GetRequiredService(type);
            else if(bindable is VisualElement visualElement)
            {
                void OnHandlerChanged(object sender, EventArgs e)
                {
                    visualElement.HandlerChanged -= OnHandlerChanged;
                    services = GetServices(visualElement);
                    visualElement.BindingContext = services.GetRequiredService(type);
                }
                visualElement.HandlerChanged += OnHandlerChanged;
            }
        }

        if (bindable is Shell || bindable is Window)
            return;

        else if (bindable is Page page)
            page.Behaviors.Add(new AppLifecycleBehavior { Page = page, View = bindable });

        else
        {
            void OnParentSet(object sender, EventArgs args)
            {
                // This shouldn't ever happen, but provides cleaner approach
                if (sender is not Element element)
                    return;

                element.ParentChanged -= OnParentSet;
                if (element.Parent is Page page)
                    page.Behaviors.Add(new AppLifecycleBehavior { Page = page, View = bindable });
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

            if (bindable is Element element)
                element.ParentChanged += OnParentSet;
        }
    }

    private static IServiceProvider GetServices(BindableObject bindable)
    {
        if (bindable is null)
            return null;
        else if (bindable is Shell shell)
            return shell.Handler?.MauiContext?.Services ?? GetServices(shell.Parent);
        else if (bindable is Window window)
            return window.Handler?.MauiContext?.Services ?? Application.Current.Handler.MauiContext.Services;

        return GetServices(Shell.Current);
    }
}

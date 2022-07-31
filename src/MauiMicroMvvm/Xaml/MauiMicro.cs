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

    private static void OnMvvmChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue is bool autowire && autowire)
        {
            var type = MvvmMapper.GetViewModelType(bindable.GetType());

            var shell = Shell.Current;
            bindable.BindingContext = shell.Handler.MauiContext.Services.GetRequiredService(type);
        }

        if (bindable is Page page)
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
}

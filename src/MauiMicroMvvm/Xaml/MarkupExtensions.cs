using MauiMicroMvvm.Xaml;

namespace Microsoft.Maui.Controls;

public static class MarkupExtensions
{
    public static T Autowire<T>(this T element, bool autowire)
        where T : VisualElement
    {
        MauiMicro.SetAutowire(element, autowire);
        return element;
    }
}

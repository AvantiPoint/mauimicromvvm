using System.ComponentModel;

#nullable enable
namespace MauiMicroMvvm.Internals;

internal interface IViewFactory
{
    TView CreateView<TView>()
        where TView : VisualElement;
    VisualElement CreateView(string key);
    TView Configure<TView>(TView view)
        where TView : VisualElement;
}

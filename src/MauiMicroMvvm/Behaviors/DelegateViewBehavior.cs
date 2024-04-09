namespace MauiMicroMvvm.Behaviors;

internal sealed class DelegateViewBehavior<TView>(Action<IServiceProvider, TView> onAttached, Action<IServiceProvider, TView> onDetached) : Behavior<TView>
    where TView : VisualElement
{
    private readonly Action<IServiceProvider, TView> _onAttached = onAttached;
    private readonly Action<IServiceProvider, TView> _onDetached = onDetached;

    protected override void OnAttachedTo(TView bindable)
    {
        base.OnAttachedTo(bindable);
        var serviceProvider = bindable.Handler?.MauiContext?.Services;
        ArgumentNullException.ThrowIfNull(serviceProvider);
        _onAttached(serviceProvider, bindable);
    }

    protected override void OnDetachingFrom(TView bindable)
    {
        base.OnDetachingFrom(bindable);
        var serviceProvider = bindable.Handler?.MauiContext?.Services;
        ArgumentNullException.ThrowIfNull(serviceProvider);
        _onDetached(serviceProvider, bindable);
    }
}

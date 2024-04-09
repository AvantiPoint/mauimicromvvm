namespace MauiMicroMvvm.Behaviors;

internal class RegisteredBehavior<TView, TBehavior>(IServiceProvider Services) : IRegisteredBehavior
    where TView : VisualElement
    where TBehavior : Behavior
{
    public Type ViewType => typeof(TView);

    public Behavior GetBehavior() => Services.GetRequiredService<TBehavior>();
}

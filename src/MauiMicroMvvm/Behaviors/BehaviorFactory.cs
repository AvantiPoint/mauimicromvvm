namespace MauiMicroMvvm.Behaviors;

public sealed class BehaviorFactory : IBehaviorFactory
{
    private readonly IEnumerable<IRegisteredBehavior> _behaviors;

    public BehaviorFactory(IServiceProvider services, IEnumerable<IRegisteredBehavior> behaviors)
    {
        ArgumentNullException.ThrowIfNull(services);
        _behaviors = behaviors ?? [];
    }

    public void ApplyBehaviors(VisualElement element)
    {
        foreach (var registration in _behaviors)
        {
            if (!registration.ViewType.IsAssignableFrom(element.GetType()))
                continue;

            var behavior = registration.GetBehavior();
            if (behavior is not null)
                element.Behaviors.Add(behavior);
        }
    }
}

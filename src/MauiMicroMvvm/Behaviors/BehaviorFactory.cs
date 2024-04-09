namespace MauiMicroMvvm.Behaviors;

public sealed class BehaviorFactory : IBehaviorFactory
{
    private readonly IEnumerable<IRegisteredBehavior> _behaviors;
    private readonly IServiceProvider _services;

    public BehaviorFactory(IServiceProvider services, IEnumerable<IRegisteredBehavior> behaviors)
    {
        ArgumentNullException.ThrowIfNull(services);
        _behaviors = behaviors ?? [];
        _services = services;
    }

    public void ApplyBehaviors(VisualElement element)
    {
        foreach (var registration in _behaviors)
        {
            if (!registration.ViewType.IsAssignableFrom(registration.ViewType))
                continue;

            var behavior = registration.GetBehavior();
            if (behavior is not null)
                element.Behaviors.Add(behavior);
        }
    }
}

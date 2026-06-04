namespace MauiMicroMvvm.Behaviors;

public sealed class BehaviorFactory : IBehaviorFactory
{
    private readonly IEnumerable<IRegisteredBehavior> _behaviors;

    public BehaviorFactory(IEnumerable<IRegisteredBehavior> behaviors)
    {
        _behaviors = behaviors ?? [];
    }

    public void ApplyBehaviors(VisualElement element)
    {
        foreach (var registration in _behaviors)
        {
            if (!registration.ViewType.IsAssignableFrom(element.GetType()))
                continue;

            var behavior = registration.GetBehavior();
            if (behavior is not null && !HasBehavior(element, behavior.GetType()))
                element.Behaviors.Add(behavior);
        }
    }

    private static bool HasBehavior(VisualElement element, Type behaviorType)
    {
        return element.Behaviors.Any(behavior => behavior.GetType() == behaviorType);
    }
}

namespace MauiMicroMvvm.Behaviors;

public interface IRegisteredBehavior
{
    Type ViewType { get; }
    Behavior GetBehavior();
}

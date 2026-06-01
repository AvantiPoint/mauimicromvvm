using Microsoft.Maui.Controls;

namespace MauiMicroMvvm.Tests.Mocks;

internal class TrackingLabelBehavior : Behavior<Label>
{
    public bool Attached { get; private set; }

    protected override void OnAttachedTo(Label bindable)
    {
        base.OnAttachedTo(bindable);
        Attached = true;
    }

    protected override void OnDetachingFrom(Label bindable)
    {
        base.OnDetachingFrom(bindable);
        Attached = false;
    }
}

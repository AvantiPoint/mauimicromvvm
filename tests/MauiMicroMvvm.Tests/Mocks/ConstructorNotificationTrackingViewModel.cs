using MauiMicroMvvm;

namespace MauiMicroMvvm.Tests.Mocks;

internal class ConstructorNotificationTrackingViewModel : MauiMicroViewModel
{
    public ConstructorNotificationTrackingViewModel(ViewModelContext context) : base(context)
    {
    }

    public int PropertyChangingCallCount { get; private set; }
    public int PropertyChangedCallCount { get; private set; }

    protected override void RaisePropertyChanging(string propertyName)
    {
        PropertyChangingCallCount++;
        base.RaisePropertyChanging(propertyName);
    }

    protected override void RaisePropertyChanged(string propertyName)
    {
        PropertyChangedCallCount++;
        base.RaisePropertyChanged(propertyName);
    }
}

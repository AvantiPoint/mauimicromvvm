using MauiMicroMvvm.Common;

namespace MauiMicroMvvm.Tests.Mocks;

internal class TestViewLifecycle : IViewLifecycle
{
    public bool OnAppearingCalled { get; private set; }
    public bool OnDisappearingCalled { get; private set; }

    public void OnAppearing()
    {
        OnAppearingCalled = true;
    }

    public void OnDisappearing()
    {
        OnDisappearingCalled = true;
    }
}

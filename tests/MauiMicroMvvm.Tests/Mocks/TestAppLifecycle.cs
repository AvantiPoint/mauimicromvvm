using MauiMicroMvvm.Common;

namespace MauiMicroMvvm.Tests.Mocks;

internal class TestAppLifecycle : IAppLifecycle
{
    public bool OnResumeCalled { get; private set; }
    public bool OnSleepCalled { get; private set; }

    public void OnResume()
    {
        OnResumeCalled = true;
    }

    public void OnSleep()
    {
        OnSleepCalled = true;
    }
}

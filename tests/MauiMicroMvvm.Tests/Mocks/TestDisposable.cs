namespace MauiMicroMvvm.Tests.Mocks;

internal class TestDisposable : IDisposable
{
    public bool Disposed { get; private set; }

    public void Dispose()
    {
        Disposed = true;
    }
}

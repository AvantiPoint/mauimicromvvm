namespace MauiMicroMvvm.Tests.Mocks;

internal class TestAsyncDisposable : IAsyncDisposable
{
    public bool Disposed { get; private set; }

    public ValueTask DisposeAsync()
    {
        Disposed = true;
        return ValueTask.CompletedTask;
    }
}

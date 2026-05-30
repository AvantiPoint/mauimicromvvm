using FluentAssertions;
using MauiMicroMvvm;
using MauiMicroMvvm.Common;
using Microsoft.Maui.Controls;
using Xunit;

namespace MauiMicroMvvm.Tests;

public class MvvmHelpersTests
{
    private class TestPage : Page { }
    private class TestDisposable : IDisposable
    {
        public bool Disposed { get; private set; }

        public void Dispose()
        {
            Disposed = true;
        }
    }

    private class TestAsyncDisposable : IAsyncDisposable
    {
        public bool Disposed { get; private set; }

        public ValueTask DisposeAsync()
        {
            Disposed = true;
            return ValueTask.CompletedTask;
        }
    }

    private class TestViewLifecycle : IViewLifecycle
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

    private class TestAppLifecycle : IAppLifecycle
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

    [Fact]
    public void InvokeViewViewModelAction_ShouldInvoke_WhenValueIsType()
    {
        // Arrange
        var lifecycle = new TestViewLifecycle();
        var invoked = false;

        // Act
        MvvmHelpers.InvokeViewViewModelAction<IViewLifecycle>(lifecycle, x =>
        {
            x.OnAppearing();
            invoked = true;
        });

        // Assert
        invoked.Should().BeTrue();
        lifecycle.OnAppearingCalled.Should().BeTrue();
    }

    [Fact]
    public void InvokeViewViewModelAction_ShouldNotInvoke_WhenValueIsNotType()
    {
        // Arrange
        var value = new object();
        var invoked = false;

        // Act
        MvvmHelpers.InvokeViewViewModelAction<IViewLifecycle>(value, _ => invoked = true);

        // Assert
        invoked.Should().BeFalse();
    }

    [Fact]
    public void InvokeViewViewModelAction_ShouldInvoke_WhenBindingContextIsType()
    {
        // Arrange
        var lifecycle = new TestViewLifecycle();
        var page = new TestPage { BindingContext = lifecycle };
        var invoked = false;

        // Act
        MvvmHelpers.InvokeViewViewModelAction<IViewLifecycle>(page, x =>
        {
            x.OnAppearing();
            invoked = true;
        });

        // Assert
        invoked.Should().BeTrue();
        lifecycle.OnAppearingCalled.Should().BeTrue();
    }

    [Fact]
    public void InvokeViewViewModelAction_ShouldHandleNull()
    {
        // Arrange & Act
        var exception = Record.Exception(() =>
            MvvmHelpers.InvokeViewViewModelAction<IViewLifecycle>(null, _ => { }));

        // Assert
        exception.Should().BeNull();
    }

    [Fact]
    public async Task InvokeViewViewModelActionAsync_ShouldInvoke_WhenValueIsType()
    {
        // Arrange
        var lifecycle = new TestViewLifecycle();
        var invoked = false;

        // Act
        await MvvmHelpers.InvokeViewViewModelActionAsync<IViewLifecycle>(lifecycle, async x =>
        {
            x.OnAppearing();
            await Task.CompletedTask;
            invoked = true;
        });

        // Assert
        invoked.Should().BeTrue();
        lifecycle.OnAppearingCalled.Should().BeTrue();
    }

    [Fact]
    public async Task InvokeViewViewModelActionAsync_ShouldNotInvoke_WhenValueIsNotType()
    {
        // Arrange
        var value = new object();
        var invoked = false;

        // Act
        await MvvmHelpers.InvokeViewViewModelActionAsync<IViewLifecycle>(value, async _ =>
        {
            await Task.CompletedTask;
            invoked = true;
        });

        // Assert
        invoked.Should().BeFalse();
    }

    [Fact]
    public async Task InvokeViewViewModelActionAsync_ShouldInvoke_WhenBindingContextIsType()
    {
        // Arrange
        var lifecycle = new TestViewLifecycle();
        var page = new TestPage { BindingContext = lifecycle };
        var invoked = false;

        // Act
        await MvvmHelpers.InvokeViewViewModelActionAsync<IViewLifecycle>(page, async x =>
        {
            x.OnAppearing();
            await Task.CompletedTask;
            invoked = true;
        });

        // Assert
        invoked.Should().BeTrue();
        lifecycle.OnAppearingCalled.Should().BeTrue();
    }

    [Fact]
    public async Task InvokeViewViewModelActionAsync_ShouldHandleNull()
    {
        // Arrange & Act
        var exception = await Record.ExceptionAsync(async () =>
            await MvvmHelpers.InvokeViewViewModelActionAsync<IViewLifecycle>(null, async _ => await Task.CompletedTask));

        // Assert
        exception.Should().BeNull();
    }

    [Fact]
    public void Destroy_ShouldDispose_WhenValueIsIDisposable()
    {
        // Arrange
        var disposable = new TestDisposable();

        // Act
        MvvmHelpers.Destroy(disposable);

        // Assert
        disposable.Disposed.Should().BeTrue();
    }

    [Fact]
    public void Destroy_ShouldDispose_WhenBindingContextIsIDisposable()
    {
        // Arrange
        var disposable = new TestDisposable();
        var page = new TestPage { BindingContext = disposable };

        // Act
        MvvmHelpers.Destroy(page);

        // Assert
        disposable.Disposed.Should().BeTrue();
    }

    [Fact]
    public void Destroy_ShouldHandleNull()
    {
        // Arrange & Act
        var exception = Record.Exception(() => MvvmHelpers.Destroy(null));

        // Assert
        exception.Should().BeNull();
    }

    [Fact]
    public async Task DestroyAsync_ShouldDispose_WhenValueIsIAsyncDisposable()
    {
        // Arrange
        var disposable = new TestAsyncDisposable();

        // Act
        await MvvmHelpers.DestroyAsync(disposable);

        // Assert
        disposable.Disposed.Should().BeTrue();
    }

    [Fact]
    public async Task DestroyAsync_ShouldDispose_WhenBindingContextIsIAsyncDisposable()
    {
        // Arrange
        var disposable = new TestAsyncDisposable();
        var page = new TestPage { BindingContext = disposable };

        // Act
        await MvvmHelpers.DestroyAsync(page);

        // Assert
        disposable.Disposed.Should().BeTrue();
    }

    [Fact]
    public async Task DestroyAsync_ShouldHandleNull()
    {
        // Arrange & Act
        var exception = await Record.ExceptionAsync(async () => await MvvmHelpers.DestroyAsync(null));

        // Assert
        exception.Should().BeNull();
    }

    [Fact]
    public void InvokeViewViewModelAction_ShouldHandleNestedBindableObjects()
    {
        // Arrange
        var lifecycle = new TestViewLifecycle();
        var innerPage = new TestPage { BindingContext = lifecycle };
        var outerPage = new TestPage { BindingContext = innerPage };
        var invoked = false;

        // Act
        MvvmHelpers.InvokeViewViewModelAction<IViewLifecycle>(outerPage, x =>
        {
            x.OnAppearing();
            invoked = true;
        });

        // Assert
        invoked.Should().BeTrue();
        lifecycle.OnAppearingCalled.Should().BeTrue();
    }
}



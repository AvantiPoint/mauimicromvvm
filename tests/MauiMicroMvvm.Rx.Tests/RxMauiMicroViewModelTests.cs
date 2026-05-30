using FluentAssertions;
using MauiMicroMvvm;
using Microsoft.Extensions.Logging;
using Moq;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Xunit;

namespace MauiMicroMvvm.Rx.Tests;

public class RxMauiMicroViewModelTests
{
    private class TestRxViewModel : RxMauiMicroViewModel
    {
        public TestRxViewModel(ViewModelContext context) : base(context)
        {
        }

        public new IObservable<ViewLifecycleState> ViewLifecycle => base.ViewLifecycle;
        public new IObservable<AppLifecycleState> AppLifecycle => base.AppLifecycle;
        public new IObservable<IDictionary<string, object>> OnParametersSet => base.OnParametersSet;
        public new CompositeDisposable Disposables => base.Disposables;
    }

    private static ViewModelContext CreateContext()
    {
        var loggerFactory = Mock.Of<ILoggerFactory>();
        var navigation = Mock.Of<INavigation>();
        var pageDialogs = Mock.Of<IPageDialogs>();
        return new ViewModelContext(loggerFactory, navigation, pageDialogs);
    }

    [Fact]
    public void Constructor_ShouldInitializeContext()
    {
        // Arrange
        var context = CreateContext();

        // Act
        var viewModel = new TestRxViewModel(context);

        // Assert
        viewModel.IsBusy.Should().BeFalse();
        viewModel.IsNotBusy.Should().BeTrue();
        viewModel.IsDisposed.Should().BeFalse();
    }

    [Fact]
    public void IsBusy_ShouldDefaultToFalse()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestRxViewModel(context);

        // Assert
        viewModel.IsBusy.Should().BeFalse();
    }

    [Fact]
    public void IsNotBusy_ShouldDefaultToTrue()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestRxViewModel(context);

        // Assert
        viewModel.IsNotBusy.Should().BeTrue();
    }

    [Fact]
    public void OnFirstLoad_ShouldEmitFirstLoadToViewLifecycle()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestRxViewModel(context);
        ViewLifecycleState? receivedState = null;
        using var subscription = viewModel.ViewLifecycle.Subscribe(state => receivedState = state);

        // Act
        ((IViewModelActivation)viewModel).OnFirstLoad();

        // Assert
        receivedState.Should().Be(ViewLifecycleState.FirstLoad);
    }

    [Fact]
    public void OnAppearing_ShouldEmitAppearingToViewLifecycle()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestRxViewModel(context);
        ViewLifecycleState? receivedState = null;
        using var subscription = viewModel.ViewLifecycle.Subscribe(state => receivedState = state);

        // Act
        ((IViewLifecycle)viewModel).OnAppearing();

        // Assert
        receivedState.Should().Be(ViewLifecycleState.Appearing);
    }

    [Fact]
    public void OnDisappearing_ShouldEmitDisappearingToViewLifecycle()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestRxViewModel(context);
        ViewLifecycleState? receivedState = null;
        using var subscription = viewModel.ViewLifecycle.Subscribe(state => receivedState = state);

        // Act
        ((IViewLifecycle)viewModel).OnDisappearing();

        // Assert
        receivedState.Should().Be(ViewLifecycleState.Disappearing);
    }

    [Fact]
    public void OnResume_ShouldEmitResumeToAppLifecycle()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestRxViewModel(context);
        AppLifecycleState? receivedState = null;
        using var subscription = viewModel.AppLifecycle.Subscribe(state => receivedState = state);

        // Act
        ((IAppLifecycle)viewModel).OnResume();

        // Assert
        receivedState.Should().Be(AppLifecycleState.Resume);
    }

    [Fact]
    public void OnSleep_ShouldEmitSleepToAppLifecycle()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestRxViewModel(context);
        AppLifecycleState? receivedState = null;
        using var subscription = viewModel.AppLifecycle.Subscribe(state => receivedState = state);

        // Act
        ((IAppLifecycle)viewModel).OnSleep();

        // Assert
        receivedState.Should().Be(AppLifecycleState.Sleep);
    }

    [Fact]
    public void ApplyQueryAttributes_ShouldEmitQueryParametersToOnParametersSet()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestRxViewModel(context);
        IDictionary<string, object>? receivedQuery = null;
        using var subscription = viewModel.OnParametersSet.Subscribe(query => receivedQuery = query);
        var query = new Dictionary<string, object> { { "key", "value" } };

        // Act
        ((IQueryAttributable)viewModel).ApplyQueryAttributes(query);

        // Assert
        receivedQuery.Should().NotBeNull();
        receivedQuery.Should().BeEquivalentTo(query);
    }

    [Fact]
    public void ApplyQueryAttributes_WithNull_ShouldEmitNullToOnParametersSet()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestRxViewModel(context);
        IDictionary<string, object>? receivedQuery = null;
        using var subscription = viewModel.OnParametersSet.Subscribe(query => receivedQuery = query);

        // Act
        ((IQueryAttributable)viewModel).ApplyQueryAttributes(null!);

        // Assert
        receivedQuery.Should().BeNull();
    }

    [Fact]
    public void Dispose_ShouldSetIsDisposed()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestRxViewModel(context);

        // Act
        viewModel.Dispose();

        // Assert
        viewModel.IsDisposed.Should().BeTrue();
    }

    [Fact]
    public void Dispose_ShouldDisposeCompositeDisposables()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestRxViewModel(context);
        var disposed = false;
        viewModel.Disposables.Add(Disposable.Create(() => disposed = true));

        // Act
        viewModel.Dispose();

        // Assert
        disposed.Should().BeTrue();
        viewModel.Disposables.IsDisposed.Should().BeTrue();
    }

    [Fact]
    public void Dispose_ShouldOnlyDisposeOnce()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestRxViewModel(context);
        var disposeCount = 0;
        viewModel.Disposables.Add(Disposable.Create(() => disposeCount++));

        // Act
        viewModel.Dispose();
        viewModel.Dispose();

        // Assert
        disposeCount.Should().Be(1);
        viewModel.IsDisposed.Should().BeTrue();
    }

    [Fact]
    public void ViewLifecycle_ShouldBeObservable()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestRxViewModel(context);
        var states = new List<ViewLifecycleState>();
        using var subscription = viewModel.ViewLifecycle.Subscribe(state => states.Add(state));

        // Act
        ((IViewModelActivation)viewModel).OnFirstLoad();
        ((IViewLifecycle)viewModel).OnAppearing();
        ((IViewLifecycle)viewModel).OnDisappearing();

        // Assert
        states.Should().HaveCount(3);
        states.Should().ContainInOrder(
            ViewLifecycleState.FirstLoad,
            ViewLifecycleState.Appearing,
            ViewLifecycleState.Disappearing);
    }

    [Fact]
    public void AppLifecycle_ShouldBeObservable()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestRxViewModel(context);
        var states = new List<AppLifecycleState>();
        using var subscription = viewModel.AppLifecycle.Subscribe(state => states.Add(state));

        // Act
        ((IAppLifecycle)viewModel).OnResume();
        ((IAppLifecycle)viewModel).OnSleep();

        // Assert
        states.Should().HaveCount(2);
        states.Should().ContainInOrder(
            AppLifecycleState.Resume,
            AppLifecycleState.Sleep);
    }

    [Fact]
    public void OnParametersSet_ShouldBeObservable()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestRxViewModel(context);
        var queries = new List<IDictionary<string, object>?>();
        using var subscription = viewModel.OnParametersSet.Subscribe(query => queries.Add(query));
        var query1 = new Dictionary<string, object> { { "key1", "value1" } };
        var query2 = new Dictionary<string, object> { { "key2", "value2" } };

        // Act
        ((IQueryAttributable)viewModel).ApplyQueryAttributes(query1);
        ((IQueryAttributable)viewModel).ApplyQueryAttributes(query2);

        // Assert
        queries.Should().HaveCount(2);
        queries[0].Should().BeEquivalentTo(query1);
        queries[1].Should().BeEquivalentTo(query2);
    }
}


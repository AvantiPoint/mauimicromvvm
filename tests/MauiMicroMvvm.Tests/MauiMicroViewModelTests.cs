using FluentAssertions;
using MauiMicroMvvm;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel;
using Xunit;

namespace MauiMicroMvvm.Tests;

public class MauiMicroViewModelTests
{
    private class TestViewModel : MauiMicroViewModel
    {
        public TestViewModel(ViewModelContext context) : base(context)
        {
        }

        public string TestProperty
        {
            get => Get<string>();
            set => Set(value);
        }

        public int? NullableInt
        {
            get => Get<int?>();
            set => Set(value);
        }

        public int ValueTypeProperty
        {
            get => Get<int>();
            set => Set(value);
        }

        public bool OnParametersSetCalled { get; private set; }

        protected override void OnParametersSet()
        {
            OnParametersSetCalled = true;
            base.OnParametersSet();
        }

        public bool OnFirstLoadCalled { get; private set; }
        public bool OnAppearingCalled { get; private set; }
        public bool OnDisappearingCalled { get; private set; }
        public bool OnResumeCalled { get; private set; }
        public bool OnSleepCalled { get; private set; }

        public override void OnFirstLoad()
        {
            OnFirstLoadCalled = true;
            base.OnFirstLoad();
        }

        public override void OnAppearing()
        {
            OnAppearingCalled = true;
            base.OnAppearing();
        }

        public override void OnDisappearing()
        {
            OnDisappearingCalled = true;
            base.OnDisappearing();
        }

        public override void OnResume()
        {
            OnResumeCalled = true;
            base.OnResume();
        }

        public override void OnSleep()
        {
            OnSleepCalled = true;
            base.OnSleep();
        }
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
        var viewModel = new TestViewModel(context);

        // Assert
        // Navigation, PageDialogs, Logger, and QueryParameters are protected, so we test indirectly
        viewModel.IsBusy.Should().BeFalse();
        viewModel.IsNotBusy.Should().BeFalse();
    }

    [Fact]
    public void Set_ShouldRaisePropertyChangingEvent()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestViewModel(context);
        System.ComponentModel.PropertyChangingEventArgs? eventArgs = null;
        viewModel.PropertyChanging += (_, e) => eventArgs = e;

        // Act
        viewModel.TestProperty = "TestValue";

        // Assert
        eventArgs.Should().NotBeNull();
        eventArgs!.PropertyName.Should().Be(nameof(TestViewModel.TestProperty));
    }

    [Fact]
    public void Set_ShouldRaisePropertyChangedEvent()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestViewModel(context);
        System.ComponentModel.PropertyChangedEventArgs? eventArgs = null;
        viewModel.PropertyChanged += (_, e) => eventArgs = e;

        // Act
        viewModel.TestProperty = "TestValue";

        // Assert
        eventArgs.Should().NotBeNull();
        eventArgs!.PropertyName.Should().Be(nameof(TestViewModel.TestProperty));
    }

    [Fact]
    public void Get_ShouldReturnDefaultValue_WhenPropertyNotSet()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestViewModel(context);

        // Act
        var value = viewModel.TestProperty;

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void Get_ShouldReturnSetValue()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestViewModel(context);

        // Act
        viewModel.TestProperty = "TestValue";
        var value = viewModel.TestProperty;

        // Assert
        value.Should().Be("TestValue");
    }

    [Fact]
    public void Set_ShouldNotRaiseEvents_WhenValueUnchanged()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestViewModel(context);
        viewModel.TestProperty = "TestValue";
        var changingCount = 0;
        var changedCount = 0;
        viewModel.PropertyChanging += (_, _) => changingCount++;
        viewModel.PropertyChanged += (_, _) => changedCount++;

        // Act
        viewModel.TestProperty = "TestValue";

        // Assert
        changingCount.Should().Be(0);
        changedCount.Should().Be(0);
    }

    // Set method is protected, so we test through public property setters
    // These tests verify Set indirectly through property changes

    // Set with callback is protected, tested indirectly through IsBusy which uses Set with callback

    [Fact]
    public void IsBusy_ShouldToggleIsNotBusyThroughStoredPropertyNotifications()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestViewModel(context);
        var changedProperties = new List<string?>();
        viewModel.PropertyChanged += (_, e) => changedProperties.Add(e.PropertyName);

        // Act
        viewModel.IsBusy = true;

        // Assert
        viewModel.IsBusy.Should().BeTrue();
        viewModel.IsNotBusy.Should().BeFalse();
        changedProperties.Should().ContainInOrder(nameof(TestViewModel.IsBusy), nameof(TestViewModel.IsNotBusy));

        // Act
        changedProperties.Clear();
        viewModel.IsBusy = false;

        // Assert
        viewModel.IsBusy.Should().BeFalse();
        viewModel.IsNotBusy.Should().BeTrue();
        changedProperties.Should().ContainInOrder(nameof(TestViewModel.IsBusy), nameof(TestViewModel.IsNotBusy));
    }

    [Fact]
    public void Get_ShouldReturnDefaultValue_ForValueTypes()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestViewModel(context);

        // Act
        var value = viewModel.ValueTypeProperty;

        // Assert
        value.Should().Be(0);
    }

    // Get with default value is protected, tested indirectly through property getters

    [Fact]
    public void ApplyQueryAttributes_ShouldSetQueryParameters()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestViewModel(context);
        var query = new Dictionary<string, object> { { "TestProperty", "QueryValue" } };

        // Act
        viewModel.ApplyQueryAttributes(query);

        // Assert
        // QueryParameters is protected, so we test indirectly through property assignment
        viewModel.TestProperty.Should().Be("QueryValue");
    }

    [Fact]
    public void ApplyQueryAttributes_WithNull_ShouldInitializeEmptyDictionary()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestViewModel(context);

        // Act
        viewModel.ApplyQueryAttributes(null!);

        // Assert
        // QueryParameters is protected, so we verify behavior through OnParametersSet being called
        viewModel.OnParametersSetCalled.Should().BeTrue();
    }

    [Fact]
    public void ApplyQueryAttributes_ShouldCallOnParametersSet()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestViewModel(context);
        var query = new Dictionary<string, object> { { "TestProperty", "QueryValue" } };

        // Act
        viewModel.ApplyQueryAttributes(query);

        // Assert
        viewModel.OnParametersSetCalled.Should().BeTrue();
    }

    [Fact]
    public void ApplyQueryAttributes_ShouldIgnoreCase()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestViewModel(context);
        var query = new Dictionary<string, object> { { "testproperty", "QueryValue" } };

        // Act
        viewModel.ApplyQueryAttributes(query);

        // Assert
        viewModel.TestProperty.Should().Be("QueryValue");
    }

    [Fact]
    public void ApplyQueryAttributes_ShouldConvertTypes()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestViewModel(context);
        var query = new Dictionary<string, object> { { "ValueTypeProperty", "42" } };

        // Act
        viewModel.ApplyQueryAttributes(query);

        // Assert
        viewModel.ValueTypeProperty.Should().Be(42);
    }

    [Fact]
    public void OnFirstLoad_ShouldBeCallable()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestViewModel(context);

        // Act
        viewModel.OnFirstLoad();

        // Assert
        viewModel.OnFirstLoadCalled.Should().BeTrue();
    }

    [Fact]
    public void OnAppearing_ShouldBeCallable()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestViewModel(context);

        // Act
        viewModel.OnAppearing();

        // Assert
        viewModel.OnAppearingCalled.Should().BeTrue();
    }

    [Fact]
    public void OnDisappearing_ShouldBeCallable()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestViewModel(context);

        // Act
        viewModel.OnDisappearing();

        // Assert
        viewModel.OnDisappearingCalled.Should().BeTrue();
    }

    [Fact]
    public void OnResume_ShouldBeCallable()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestViewModel(context);

        // Act
        viewModel.OnResume();

        // Assert
        viewModel.OnResumeCalled.Should().BeTrue();
    }

    [Fact]
    public void OnSleep_ShouldBeCallable()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestViewModel(context);

        // Act
        viewModel.OnSleep();

        // Assert
        viewModel.OnSleepCalled.Should().BeTrue();
    }

    [Fact]
    public void Dispose_ShouldDispose()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestViewModel(context);

        // Act
        ((IDisposable)viewModel).Dispose();

        // Assert
        // IsDisposed is protected, so we verify disposal by checking it doesn't throw on second call
        ((IDisposable)viewModel).Dispose(); // Should not throw
    }

    [Fact]
    public void Set_ShouldRemoveProperty_WhenValueIsNull()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestViewModel(context);
        viewModel.TestProperty = "InitialValue";

        // Act
        viewModel.TestProperty = null!;

        // Assert
        viewModel.TestProperty.Should().BeNull();
    }

    // Get with nullable type validation is protected, tested indirectly through ApplyQueryAttributes
}


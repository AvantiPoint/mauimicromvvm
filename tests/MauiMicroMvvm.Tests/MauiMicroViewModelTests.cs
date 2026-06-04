using FluentAssertions;
using MauiMicroMvvm;
using MauiMicroMvvm.Tests.Mocks;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel;
using Xunit;

namespace MauiMicroMvvm.Tests;

public class MauiMicroViewModelTests
{

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
        var viewModel = new TestMauiMicroViewModel(context);

        // Assert
        // Navigation, PageDialogs, Logger, and QueryParameters are protected, so we test indirectly
        viewModel.IsBusy.Should().BeFalse();
        viewModel.IsNotBusy.Should().BeTrue();
    }

    [Fact]
    public void Constructor_ShouldInitializeIsNotBusyWithoutRaisingVirtualNotifications()
    {
        // Arrange
        var context = CreateContext();

        // Act
        var viewModel = new ConstructorNotificationTrackingViewModel(context);

        // Assert
        viewModel.IsBusy.Should().BeFalse();
        viewModel.IsNotBusy.Should().BeTrue();
        viewModel.PropertyChangingCallCount.Should().Be(0);
        viewModel.PropertyChangedCallCount.Should().Be(0);
    }

    [Fact]
    public void Set_ShouldRaisePropertyChangingEvent()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestMauiMicroViewModel(context);
        System.ComponentModel.PropertyChangingEventArgs? eventArgs = null;
        viewModel.PropertyChanging += (_, e) => eventArgs = e;

        // Act
        viewModel.TestProperty = "TestValue";

        // Assert
        eventArgs.Should().NotBeNull();
        eventArgs!.PropertyName.Should().Be(nameof(TestMauiMicroViewModel.TestProperty));
    }

    [Fact]
    public void Set_ShouldRaisePropertyChangedEvent()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestMauiMicroViewModel(context);
        System.ComponentModel.PropertyChangedEventArgs? eventArgs = null;
        viewModel.PropertyChanged += (_, e) => eventArgs = e;

        // Act
        viewModel.TestProperty = "TestValue";

        // Assert
        eventArgs.Should().NotBeNull();
        eventArgs!.PropertyName.Should().Be(nameof(TestMauiMicroViewModel.TestProperty));
    }

    [Fact]
    public void Get_ShouldReturnDefaultValue_WhenPropertyNotSet()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestMauiMicroViewModel(context);

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
        var viewModel = new TestMauiMicroViewModel(context);

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
        var viewModel = new TestMauiMicroViewModel(context);
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
        var viewModel = new TestMauiMicroViewModel(context);
        var changedProperties = new List<string?>();
        viewModel.PropertyChanged += (_, e) => changedProperties.Add(e.PropertyName);

        viewModel.IsBusy.Should().BeFalse();
        viewModel.IsNotBusy.Should().BeTrue();

        // Act
        viewModel.IsBusy = true;

        // Assert
        viewModel.IsBusy.Should().BeTrue();
        viewModel.IsNotBusy.Should().BeFalse();
        changedProperties.Should().ContainInOrder(nameof(TestMauiMicroViewModel.IsBusy), nameof(TestMauiMicroViewModel.IsNotBusy));

        // Act
        changedProperties.Clear();
        viewModel.IsBusy = false;

        // Assert
        viewModel.IsBusy.Should().BeFalse();
        viewModel.IsNotBusy.Should().BeTrue();
        changedProperties.Should().ContainInOrder(nameof(TestMauiMicroViewModel.IsBusy), nameof(TestMauiMicroViewModel.IsNotBusy));
    }

    [Fact]
    public void Get_ShouldReturnDefaultValue_ForValueTypes()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestMauiMicroViewModel(context);

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
        var viewModel = new TestMauiMicroViewModel(context);
        var query = new Dictionary<string, object> { { "TestProperty", "QueryValue" } };

        // Act
        viewModel.ApplyQueryAttributes(query);

        // Assert
        // QueryParameters is protected, so we test indirectly through property assignment
        viewModel.TestProperty.Should().Be("QueryValue");
    }

    [Fact]
    public void ApplyQueryAttributes_WithNull_ShouldPreserveOriginalEarlyReturnSemantics()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestMauiMicroViewModel(context);

        // Act
        viewModel.ApplyQueryAttributes(null!);

        // Assert
        viewModel.OnParametersSetCalled.Should().BeFalse();
    }

    [Fact]
    public void ApplyQueryAttributes_ShouldCallOnParametersSet()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestMauiMicroViewModel(context);
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
        var viewModel = new TestMauiMicroViewModel(context);
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
        var viewModel = new TestMauiMicroViewModel(context);
        var query = new Dictionary<string, object> { { "ValueTypeProperty", "42" } };

        // Act
        viewModel.ApplyQueryAttributes(query);

        // Assert
        viewModel.ValueTypeProperty.Should().Be(42);
    }

    [Fact]
    public void ApplyQueryAttributes_ShouldDeserializeComplexTypesFromJson()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestMauiMicroViewModel(context);
        var query = new Dictionary<string, object>
        {
            { "ComplexProperty", "{\"Name\":\"Widget\",\"Count\":7}" },
        };

        // Act
        viewModel.ApplyQueryAttributes(query);

        // Assert
        viewModel.ComplexProperty.Should().NotBeNull();
        viewModel.ComplexProperty!.Name.Should().Be("Widget");
        viewModel.ComplexProperty.Count.Should().Be(7);
    }

    [Fact]
    public void ApplyQueryAttributes_WithSingleSetterError_ShouldThrowQuerystringPropertyException()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new QueryErrorAggregationViewModel(context);
        var query = new Dictionary<string, object>
        {
            { "UnknownProperty", "ignored" },
            { "FirstFailure", "bad" },
            { "TestProperty", "QueryValue" },
        };

        // Act
        var act = () => viewModel.ApplyQueryAttributes(query);

        // Assert
        var exception = act.Should().Throw<QuerystringPropertyException>().Which;
        exception.Message.Should().Be("Failed to set FirstFailure");
        exception.Property.Should().Be("FirstFailure");
        exception.InnerException.Should().BeOfType<InvalidOperationException>();
        viewModel.TestProperty.Should().Be("QueryValue");
        viewModel.OnParametersSetCalled.Should().BeFalse();
        viewModel.TrackingParameters.TryGetSetterCount.Should().Be(3);
    }

    [Fact]
    public void ApplyQueryAttributes_WithMultipleSetterErrors_ShouldAggregateAndContinue()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new QueryErrorAggregationViewModel(context);
        var query = new Dictionary<string, object>
        {
            { "FirstFailure", "bad" },
            { "TestProperty", "QueryValue" },
            { "SecondFailure", "worse" },
        };

        // Act
        var act = () => viewModel.ApplyQueryAttributes(query);

        // Assert
        var exception = act.Should().Throw<AggregateException>().Which;
        exception.InnerExceptions.Should().HaveCount(2);
        exception.InnerExceptions.Should().AllBeOfType<QuerystringPropertyException>();
        exception.InnerExceptions.Cast<QuerystringPropertyException>()
            .Select(error => error.Property)
            .Should().Equal("FirstFailure", "SecondFailure");
        viewModel.TestProperty.Should().Be("QueryValue");
        viewModel.OnParametersSetCalled.Should().BeFalse();
        viewModel.TrackingParameters.TryGetSetterCount.Should().Be(3);
    }

    [Fact]
    public void ApplyQueryAttributes_WithRejectedSetter_ShouldThrowAndSkipOnParametersSet()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new QueryErrorAggregationViewModel(context);
        var query = new Dictionary<string, object>
        {
            { "RejectedSetter", "bad" },
            { "TestProperty", "QueryValue" },
        };

        // Act
        var act = () => viewModel.ApplyQueryAttributes(query);

        // Assert
        var exception = act.Should().Throw<QuerystringPropertyException>().Which;
        exception.Message.Should().Be("Failed to set RejectedSetter");
        exception.Property.Should().Be("RejectedSetter");
        exception.InnerException.Should().BeOfType<InvalidOperationException>();
        viewModel.TestProperty.Should().Be("QueryValue");
        viewModel.OnParametersSetCalled.Should().BeFalse();
        viewModel.TrackingParameters.TryGetSetterCount.Should().Be(2);
    }

    [Fact]
    public void ApplyQueryAttributes_ShouldRaiseChangingBeforeChanged()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestMauiMicroViewModel(context);
        var events = new List<string>();
        viewModel.PropertyChanging += (_, e) => events.Add($"Changing:{e.PropertyName}");
        viewModel.PropertyChanged += (_, e) => events.Add($"Changed:{e.PropertyName}");
        var query = new Dictionary<string, object> { { "TestProperty", "QueryValue" } };

        // Act
        viewModel.ApplyQueryAttributes(query);

        // Assert
        events.Should().Equal(
            $"Changing:{nameof(TestMauiMicroViewModel.TestProperty)}",
            $"Changed:{nameof(TestMauiMicroViewModel.TestProperty)}");
    }

    [Fact]
    public void ApplyQueryAttributes_ShouldUseQueryParameterMapAbstraction()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new QueryLookupTrackingViewModel(context);
        var query = new Dictionary<string, object>
        {
            { "testproperty", "QueryValue" },
            { "ValueTypeProperty", "42" },
            { "UnknownProperty", "ignored" },
        };

        // Act
        viewModel.ApplyQueryAttributes(query);

        // Assert
        viewModel.TestProperty.Should().Be("QueryValue");
        viewModel.ValueTypeProperty.Should().Be(42);
        viewModel.TrackingParameters.TryGetSetterCount.Should().Be(3);
    }

    [Fact]
    public void ApplyQueryAttributes_ShouldSetThroughPropertySetter()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new SetterSideEffectViewModel(context);
        var query = new Dictionary<string, object> { { "SetterSideEffectProperty", "QueryValue" } };

        // Act
        viewModel.ApplyQueryAttributes(query);

        // Assert
        viewModel.SetterSideEffectProperty.Should().Be("QueryValue");
        viewModel.SetterCallCount.Should().Be(1);
    }

    [Fact]
    public void GetQueryParameterMap_ShouldCreateDefaultMapPerViewModelInstance()
    {
        // Arrange
        var context = CreateContext();
        var first = new CachedParametersTestViewModel(context);
        var second = new CachedParametersTestViewModel(context);

        // Act
        var firstParameters = first.GetQueryParameterMapForTest();
        var secondParameters = second.GetQueryParameterMapForTest();

        // Assert
        secondParameters.Should().NotBeSameAs(firstParameters);
        firstParameters.TryGetSetter(nameof(TestMauiMicroViewModel.TestProperty), out var setter).Should().BeTrue();
        setter.Name.Should().Be(nameof(TestMauiMicroViewModel.TestProperty));
    }

    [Fact]
    public void GetQueryParameterMap_ShouldCreateCustomMapOncePerViewModelInstance()
    {
        // Arrange
        var context = CreateContext();
        var first = new InstanceQueryParameterMapViewModel(context);
        var second = new InstanceQueryParameterMapViewModel(context);

        // Act
        var firstInitialMap = first.GetQueryParameterMapForTest();
        var firstRepeatedMap = first.GetQueryParameterMapForTest();
        var secondMap = second.GetQueryParameterMapForTest();

        // Assert
        firstRepeatedMap.Should().BeSameAs(firstInitialMap);
        secondMap.Should().NotBeSameAs(firstInitialMap);
        first.CreateQueryParameterMapCallCount.Should().Be(1);
        second.CreateQueryParameterMapCallCount.Should().Be(1);
    }

    [Fact]
    public void ApplyQueryAttributes_WithEmptyQuery_ShouldPreserveOriginalEarlyReturnSemantics()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestMauiMicroViewModel(context);

        // Act
        viewModel.ApplyQueryAttributes(new Dictionary<string, object>());

        // Assert
        viewModel.OnParametersSetCalled.Should().BeFalse();
    }

    [Fact]
    public void OnFirstLoad_ShouldBeCallable()
    {
        // Arrange
        var context = CreateContext();
        var viewModel = new TestMauiMicroViewModel(context);

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
        var viewModel = new TestMauiMicroViewModel(context);

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
        var viewModel = new TestMauiMicroViewModel(context);

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
        var viewModel = new TestMauiMicroViewModel(context);

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
        var viewModel = new TestMauiMicroViewModel(context);

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
        var viewModel = new TestMauiMicroViewModel(context);

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
        var viewModel = new TestMauiMicroViewModel(context);
        viewModel.TestProperty = "InitialValue";

        // Act
        viewModel.TestProperty = null!;

        // Assert
        viewModel.TestProperty.Should().BeNull();
    }

    // Get with nullable type validation is protected, tested indirectly through ApplyQueryAttributes
}


using FluentAssertions;
using MauiMicroMvvm.Behaviors;
using MauiMicroMvvm.Internals;
using MauiMicroMvvm.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using Moq;
using Xunit;

namespace MauiMicroMvvm.Tests;

public class ViewFactoryTests
{

    private static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddTransient<TestViewModel>();
        return services.BuildServiceProvider();
    }

    private static IBehaviorFactory CreateBehaviorFactory()
    {
        return Mock.Of<IBehaviorFactory>();
    }

    [Fact]
    public void CreateView_Generic_ShouldCreateViewInstance()
    {
        // Arrange
        var services = CreateServiceProvider();
        var mappings = Array.Empty<ViewMapping>();
        var behaviorFactory = CreateBehaviorFactory();
        var factory = new ViewFactory(services, mappings, behaviorFactory);

        // Act
        var view = factory.CreateView<TestLabel>();

        // Assert
        view.Should().NotBeNull();
        view.Should().BeOfType<TestLabel>();
    }

    [Fact]
    public void CreateView_Generic_ShouldConfigureView()
    {
        // Arrange
        var services = CreateServiceProvider();
        var mappings = Array.Empty<ViewMapping>();
        var behaviorFactory = Mock.Of<IBehaviorFactory>();
        var factory = new ViewFactory(services, mappings, behaviorFactory);

        // Act
        var view = factory.CreateView<TestLabel>();

        // Assert
        Mock.Get(behaviorFactory).Verify(b => b.ApplyBehaviors(view), Times.Once);
    }

    [Fact]
    public void CreateView_WithKey_ShouldCreateViewFromMapping()
    {
        // Arrange
        var services = CreateServiceProvider();
        var mapping = new ViewMapping("TestKey", typeof(TestLabel), typeof(TestViewModel));
        var mappings = new[] { mapping };
        var behaviorFactory = CreateBehaviorFactory();
        var factory = new ViewFactory(services, mappings, behaviorFactory);

        // Act
        var view = factory.CreateView("TestKey");

        // Assert
        view.Should().NotBeNull();
        view.Should().BeOfType<TestLabel>();
        ViewFactory.GetNavigationKey(view).Should().Be("TestKey");
    }

    [Fact]
    public void CreateView_WithKey_ShouldThrow_WhenKeyNotFound()
    {
        // Arrange
        var services = CreateServiceProvider();
        var mappings = Array.Empty<ViewMapping>();
        var behaviorFactory = CreateBehaviorFactory();
        var factory = new ViewFactory(services, mappings, behaviorFactory);

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => factory.CreateView("NonExistentKey"));
    }

    [Fact]
    public void Configure_ShouldSetBindingContext_WhenAutowireEnabled()
    {
        // Arrange
        var services = CreateServiceProvider();
        var mapping = new ViewMapping("TestKey", typeof(TestLabel), typeof(TestViewModel));
        var mappings = new[] { mapping };
        var behaviorFactory = CreateBehaviorFactory();
        var factory = new ViewFactory(services, mappings, behaviorFactory);
        var view = new TestLabel();

        // Act
        factory.Configure(view);

        // Assert
        view.BindingContext.Should().NotBeNull();
        view.BindingContext.Should().BeOfType<TestViewModel>();
    }

    [Fact]
    public void Configure_ShouldNotSetBindingContext_WhenAlreadySet()
    {
        // Arrange
        var services = CreateServiceProvider();
        var mapping = new ViewMapping("TestKey", typeof(TestLabel), typeof(TestViewModel));
        var mappings = new[] { mapping };
        var behaviorFactory = CreateBehaviorFactory();
        var factory = new ViewFactory(services, mappings, behaviorFactory);
        var view = new TestLabel();
        var existingContext = new object();
        view.BindingContext = existingContext;

        // Act
        factory.Configure(view);

        // Assert
        view.BindingContext.Should().BeSameAs(existingContext);
    }

    [Fact]
    public void GetNavigationKey_ShouldReturnSetValue()
    {
        // Arrange
        var label = new Label();
        var key = "TestKey";

        // Act
        ViewFactory.SetNavigationKey(label, key);
        var result = ViewFactory.GetNavigationKey(label);

        // Assert
        result.Should().Be(key);
    }

    [Fact]
    public void GetNavigationKey_ShouldReturnNull_WhenNotSet()
    {
        // Arrange
        var label = new Label();

        // Act
        var result = ViewFactory.GetNavigationKey(label);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Configure_Page_ShouldAddAppLifecycleBehavior()
    {
        // Arrange
        var services = CreateServiceProvider();
        var mappings = Array.Empty<ViewMapping>();
        var behaviorFactory = CreateBehaviorFactory();
        var factory = new ViewFactory(services, mappings, behaviorFactory);
        var page = new TestPage();

        // Act
        factory.Configure(page);

        // Assert
        page.Behaviors.Should().ContainSingle(b => b is AppLifecycleBehavior);
        var behavior = page.Behaviors.OfType<AppLifecycleBehavior>().Single();
        behavior.Page.Should().Be(page);
        behavior.View.Should().Be(page);
    }
}



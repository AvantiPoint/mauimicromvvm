using FluentAssertions;
using MauiMicroMvvm.Behaviors;
using MauiMicroMvvm.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using Xunit;

namespace MauiMicroMvvm.Tests;

public class BehaviorFactoryTests
{

    [Fact]
    public void Constructor_ShouldHandleNullBehaviors()
    {
        var factory = new BehaviorFactory(null!);

        factory.Should().NotBeNull();
    }

    [Fact]
    public void ApplyBehavior_ShouldRegisterBehaviorForFactoryDiscovery()
    {
        var services = new ServiceCollection();
        services.ApplyBehavior<TestLabel, TrackingLabelBehavior>();
        using var serviceProvider = services.BuildServiceProvider();

        var registrations = serviceProvider.GetServices<IRegisteredBehavior>().ToArray();

        registrations.Should().ContainSingle();
        registrations[0].ViewType.Should().Be(typeof(TestLabel));
    }

    [Fact]
    public void ApplyBehaviors_ShouldAttachMatchingBehaviorToVisualElement()
    {
        var services = new ServiceCollection();
        services.ApplyBehavior<TestLabel, TrackingLabelBehavior>();
        services.AddSingleton<IBehaviorFactory>(sp => new BehaviorFactory(sp.GetServices<IRegisteredBehavior>()));
        using var serviceProvider = services.BuildServiceProvider();
        var element = new TestLabel();

        serviceProvider.GetRequiredService<IBehaviorFactory>().ApplyBehaviors(element);

        var behavior = element.Behaviors.Should().ContainSingle().Subject.Should().BeOfType<TrackingLabelBehavior>().Subject;
        behavior.Attached.Should().BeTrue();
    }

    [Fact]
    public void ApplyBehaviors_ShouldNotAttachDuplicateBehaviorWhenConfiguredTwice()
    {
        var services = new ServiceCollection();
        services.ApplyBehavior<TestLabel, TrackingLabelBehavior>();
        services.AddSingleton<IBehaviorFactory>(sp => new BehaviorFactory(sp.GetServices<IRegisteredBehavior>()));
        using var serviceProvider = services.BuildServiceProvider();
        var element = new TestLabel();
        var factory = serviceProvider.GetRequiredService<IBehaviorFactory>();

        factory.ApplyBehaviors(element);
        factory.ApplyBehaviors(element);

        element.Behaviors.Should().ContainSingle().Which.Should().BeOfType<TrackingLabelBehavior>();
    }

    [Fact]
    public void ApplyBehaviors_ShouldAttachBehaviorRegisteredForBaseVisualElement()
    {
        var services = new ServiceCollection();
        services.ApplyBehavior<VisualElement, TrackingLabelBehavior>();
        services.AddSingleton<IBehaviorFactory>(sp => new BehaviorFactory(sp.GetServices<IRegisteredBehavior>()));
        using var serviceProvider = services.BuildServiceProvider();
        var element = new TestLabel();

        serviceProvider.GetRequiredService<IBehaviorFactory>().ApplyBehaviors(element);

        element.Behaviors.Should().ContainSingle().Which.Should().BeOfType<TrackingLabelBehavior>();
    }

    [Fact]
    public void ApplyBehaviors_ShouldNotAttachBehaviorRegisteredForDifferentVisualElementType()
    {
        var services = new ServiceCollection();
        services.ApplyBehavior<TestPage, TrackingLabelBehavior>();
        services.AddSingleton<IBehaviorFactory>(sp => new BehaviorFactory(sp.GetServices<IRegisteredBehavior>()));
        using var serviceProvider = services.BuildServiceProvider();
        var element = new TestLabel();

        serviceProvider.GetRequiredService<IBehaviorFactory>().ApplyBehaviors(element);

        element.Behaviors.Should().BeEmpty();
    }

    [Fact]
    public void ApplyBehaviors_ShouldHandleEmptyBehaviorsList()
    {
        var factory = new BehaviorFactory([]);
        var element = new TestLabel();

        factory.ApplyBehaviors(element);

        element.Behaviors.Should().BeEmpty();
    }
}

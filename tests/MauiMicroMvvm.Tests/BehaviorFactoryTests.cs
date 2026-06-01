using FluentAssertions;
using MauiMicroMvvm.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using Xunit;

namespace MauiMicroMvvm.Tests;

public class BehaviorFactoryTests
{
    private class TestBehavior : Behavior<Label>
    {
        public bool Attached { get; private set; }

        protected override void OnAttachedTo(Label bindable)
        {
            base.OnAttachedTo(bindable);
            Attached = true;
        }

        protected override void OnDetachingFrom(Label bindable)
        {
            base.OnDetachingFrom(bindable);
            Attached = false;
        }
    }

    private class TestLabel : Label { }
    private class TestPage : Page { }

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
        services.ApplyBehavior<TestLabel, TestBehavior>();
        using var serviceProvider = services.BuildServiceProvider();

        var registrations = serviceProvider.GetServices<IRegisteredBehavior>().ToArray();

        registrations.Should().ContainSingle();
        registrations[0].ViewType.Should().Be(typeof(TestLabel));
    }

    [Fact]
    public void ApplyBehaviors_ShouldAttachMatchingBehaviorToVisualElement()
    {
        var services = new ServiceCollection();
        services.ApplyBehavior<TestLabel, TestBehavior>();
        services.AddSingleton<IBehaviorFactory>(sp => new BehaviorFactory(sp.GetServices<IRegisteredBehavior>()));
        using var serviceProvider = services.BuildServiceProvider();
        var element = new TestLabel();

        serviceProvider.GetRequiredService<IBehaviorFactory>().ApplyBehaviors(element);

        var behavior = element.Behaviors.Should().ContainSingle().Subject.Should().BeOfType<TestBehavior>().Subject;
        behavior.Attached.Should().BeTrue();
    }

    [Fact]
    public void ApplyBehaviors_ShouldAttachBehaviorRegisteredForBaseVisualElement()
    {
        var services = new ServiceCollection();
        services.ApplyBehavior<VisualElement, TestBehavior>();
        services.AddSingleton<IBehaviorFactory>(sp => new BehaviorFactory(sp.GetServices<IRegisteredBehavior>()));
        using var serviceProvider = services.BuildServiceProvider();
        var element = new TestLabel();

        serviceProvider.GetRequiredService<IBehaviorFactory>().ApplyBehaviors(element);

        element.Behaviors.Should().ContainSingle().Which.Should().BeOfType<TestBehavior>();
    }

    [Fact]
    public void ApplyBehaviors_ShouldNotAttachBehaviorRegisteredForDifferentVisualElementType()
    {
        var services = new ServiceCollection();
        services.ApplyBehavior<TestPage, TestBehavior>();
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

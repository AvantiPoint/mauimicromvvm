using FluentAssertions;
using MauiMicroMvvm;
using Xunit;

namespace MauiMicroMvvm.Rx.Tests;

public class ViewLifecycleStateTests
{
    [Fact]
    public void Enum_ShouldHaveFirstLoadValue()
    {
        // Arrange & Act
        var state = ViewLifecycleState.FirstLoad;

        // Assert
        state.Should().Be(ViewLifecycleState.FirstLoad);
        ((int)state).Should().Be(0);
    }

    [Fact]
    public void Enum_ShouldHaveAppearingValue()
    {
        // Arrange & Act
        var state = ViewLifecycleState.Appearing;

        // Assert
        state.Should().Be(ViewLifecycleState.Appearing);
        ((int)state).Should().Be(1);
    }

    [Fact]
    public void Enum_ShouldHaveDisappearingValue()
    {
        // Arrange & Act
        var state = ViewLifecycleState.Disappearing;

        // Assert
        state.Should().Be(ViewLifecycleState.Disappearing);
        ((int)state).Should().Be(2);
    }

    [Fact]
    public void Enum_ShouldHaveCorrectValues()
    {
        // Assert
        Enum.GetValues<ViewLifecycleState>().Should().HaveCount(3);
        Enum.GetValues<ViewLifecycleState>().Should().Contain(ViewLifecycleState.FirstLoad);
        Enum.GetValues<ViewLifecycleState>().Should().Contain(ViewLifecycleState.Appearing);
        Enum.GetValues<ViewLifecycleState>().Should().Contain(ViewLifecycleState.Disappearing);
    }
}



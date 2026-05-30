using FluentAssertions;
using MauiMicroMvvm;
using Xunit;

namespace MauiMicroMvvm.Rx.Tests;

public class AppLifecycleStateTests
{
    [Fact]
    public void Enum_ShouldHaveResumeValue()
    {
        // Arrange & Act
        var state = AppLifecycleState.Resume;

        // Assert
        state.Should().Be(AppLifecycleState.Resume);
        ((int)state).Should().Be(0);
    }

    [Fact]
    public void Enum_ShouldHaveSleepValue()
    {
        // Arrange & Act
        var state = AppLifecycleState.Sleep;

        // Assert
        state.Should().Be(AppLifecycleState.Sleep);
        ((int)state).Should().Be(1);
    }

    [Fact]
    public void Enum_ShouldHaveCorrectValues()
    {
        // Assert
        Enum.GetValues<AppLifecycleState>().Should().HaveCount(2);
        Enum.GetValues<AppLifecycleState>().Should().Contain(AppLifecycleState.Resume);
        Enum.GetValues<AppLifecycleState>().Should().Contain(AppLifecycleState.Sleep);
    }
}



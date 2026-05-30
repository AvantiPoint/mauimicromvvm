using FluentAssertions;
using MauiMicroMvvm;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace MauiMicroMvvm.Tests;

public class ViewModelContextTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var loggerFactory = Mock.Of<ILoggerFactory>();
        var navigation = Mock.Of<INavigation>();
        var pageDialogs = Mock.Of<IPageDialogs>();

        // Act
        var context = new ViewModelContext(loggerFactory, navigation, pageDialogs);

        // Assert
        context.Logger.Should().Be(loggerFactory);
        context.Navigation.Should().Be(navigation);
        context.PageDialogs.Should().Be(pageDialogs);
    }

    [Fact]
    public void Constructor_ShouldStoreAllDependencies()
    {
        // Arrange
        var loggerFactory = new Mock<ILoggerFactory>();
        var navigation = new Mock<INavigation>();
        var pageDialogs = new Mock<IPageDialogs>();

        // Act
        var context = new ViewModelContext(loggerFactory.Object, navigation.Object, pageDialogs.Object);

        // Assert
        context.Logger.Should().BeSameAs(loggerFactory.Object);
        context.Navigation.Should().BeSameAs(navigation.Object);
        context.PageDialogs.Should().BeSameAs(pageDialogs.Object);
    }
}



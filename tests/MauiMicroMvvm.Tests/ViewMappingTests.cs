using FluentAssertions;
using MauiMicroMvvm.Internals;
using Xunit;

namespace MauiMicroMvvm.Tests;

public class ViewMappingTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var name = "TestView";
        var viewType = typeof(Label);
        var viewModelType = typeof(object);

        // Act
        var mapping = new ViewMapping(name, viewType, viewModelType);

        // Assert
        mapping.Name.Should().Be(name);
        mapping.View.Should().Be(viewType);
        mapping.ViewModel.Should().Be(viewModelType);
    }

    [Fact]
    public void Constructor_ShouldAllowNullViewModel()
    {
        // Arrange
        var name = "TestView";
        var viewType = typeof(Label);

        // Act
        var mapping = new ViewMapping(name, viewType);

        // Assert
        mapping.Name.Should().Be(name);
        mapping.View.Should().Be(viewType);
        mapping.ViewModel.Should().BeNull();
    }

    [Fact]
    public void RecordEquality_ShouldWorkCorrectly()
    {
        // Arrange
        var mapping1 = new ViewMapping("Test", typeof(Label), typeof(object));
        var mapping2 = new ViewMapping("Test", typeof(Label), typeof(object));
        var mapping3 = new ViewMapping("Other", typeof(Label), typeof(object));

        // Act & Assert
        mapping1.Should().Be(mapping2);
        mapping1.Should().NotBe(mapping3);
    }
}



# MauiMicroMvvm Unit Tests

This directory contains comprehensive unit tests for the MauiMicroMvvm framework.

## Test Coverage

The test suite covers the following components:

### Core Components
- **MauiMicroViewModel**: Property management, lifecycle events, query parameters, disposal
- **ViewModelContext**: Context initialization and dependency injection
- **DefaultNavigation**: Navigation delegation to Shell
- **ViewFactory**: View creation, configuration, and binding context management
- **ViewMapping**: View-to-ViewModel mapping records
- **BehaviorFactory**: Behavior registration and application
- **RegisteredBehavior**: Behavior type registration
- **MvvmHelpers**: Helper methods for view/viewmodel interactions and disposal
- **PageDialogs**: Dialog display abstraction
- **MauiMicroBuilderExtensions**: Service registration and extension methods

## Running Tests

To run all tests:
```bash
dotnet test
```

To run tests with coverage:
```bash
dotnet test /p:CollectCoverage=true
```

To run a specific test class:
```bash
dotnet test --filter "FullyQualifiedName~MauiMicroViewModelTests"
```

## Test Structure

Tests are organized by component, with each test file containing:
- Unit tests for the component's public API
- Edge case testing
- Null handling verification
- Event verification where applicable

## Dependencies

- **xUnit**: Test framework
- **Moq**: Mocking framework for dependencies
- **FluentAssertions**: Fluent assertion syntax for better test readability
- **Microsoft.Maui.Controls**: MAUI controls for testing

## Known Issues

- **BehaviorFactory.ApplyBehaviors**: There appears to be a bug in line 19 where it checks `!registration.ViewType.IsAssignableFrom(registration.ViewType)` which will always be false. This means all registered behaviors are currently applied regardless of view type matching. This should likely be `element.GetType().IsAssignableTo(registration.ViewType)`.



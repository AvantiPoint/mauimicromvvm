# MauiMicroMvvm.Rx Unit Tests

This directory contains comprehensive unit tests for the MauiMicroMvvm.Rx library, which provides Reactive Extensions support for the MauiMicroMvvm framework.

## Test Coverage

The test suite covers the following components:

### Core Components
- **RxMauiMicroViewModel**: Reactive ViewModel with observables for lifecycle events
  - Constructor initialization
  - IsBusy/IsNotBusy properties
  - View lifecycle observables (FirstLoad, Appearing, Disappearing)
  - App lifecycle observables (Resume, Sleep)
  - Query parameters observables
  - Disposal pattern and CompositeDisposable management
- **AppLifecycleState**: Enum for application lifecycle states
- **ViewLifecycleState**: Enum for view lifecycle states

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
dotnet test --filter "FullyQualifiedName~RxMauiMicroViewModelTests"
```

## Test Structure

Tests are organized by component, with each test file containing:
- Unit tests for the component's public API
- Reactive observable subscription and emission verification
- Lifecycle event testing
- Disposal pattern verification

## Dependencies

- **xUnit**: Test framework
- **Moq**: Mocking framework for dependencies
- **FluentAssertions**: Fluent assertion syntax for better test readability
- **ReactiveUI**: Reactive Extensions library for MAUI
- **Microsoft.Maui.Controls**: MAUI controls for testing

## Key Features Tested

- **Reactive Lifecycle Events**: All lifecycle events (View and App) emit through IObservable streams
- **Query Parameters**: Query parameter changes are exposed as observables
- **CompositeDisposable Management**: Proper disposal of reactive subscriptions
- **IsBusy/IsNotBusy**: Reactive properties that are properly synchronized



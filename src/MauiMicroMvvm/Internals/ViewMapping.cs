#nullable enable
namespace MauiMicroMvvm.Internals;

public record ViewMapping(string Name, Type View, Type? ViewModel = null);

#nullable enable
namespace MauiMicroMvvm.Internals;

internal record ViewMapping(string Name, Type View, Type? ViewModel = null);

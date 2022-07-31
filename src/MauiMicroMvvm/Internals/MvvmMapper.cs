using System.ComponentModel;

namespace MauiMicroMvvm.Internals;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class MvvmMapper
{
    private static Dictionary<string, (Type View, Type ViewModel)> _registry = new();

    internal static Type GetViewModelType(Type viewType)
    {
        var value = _registry.Values.FirstOrDefault(x => x.View == viewType);
        if (value.ViewModel is not null)
            return value.ViewModel;

        throw new Exception($"No View/ViewModel mapping could be found for {viewType.FullName}");
    }

    internal static void Register(string key, Type view, Type viewModel) =>
        _registry[key] = (view, viewModel);

    public static void Clear() => _registry.Clear();
}

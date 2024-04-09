using Microsoft.Maui.Controls;

namespace MauiMicroMvvm.Common;

public static class MvvmHelpers
{
    public static void InvokeViewViewModelAction<T>(object? value, Action<T> action)
    {
        if (value is T valueAsT)
        {
            action(valueAsT);
        }

        if (value is BindableObject bindable)
        {
            InvokeViewViewModelAction(bindable.BindingContext, action);
        }
    }

    public static async Task InvokeViewViewModelActionAsync<T>(object? value, Func<T, Task> action)
    {
        if (value is T valueAsT)
        {
            await action(valueAsT);
        }

        if (value is BindableObject bindable)
        {
            await InvokeViewViewModelActionAsync(bindable.BindingContext, action);
        }
    }

    public static void Destroy(object? page)
    {
        InvokeViewViewModelAction<IDisposable>(page, x => x.Dispose());
    }

    public static Task DestroyAsync(object? page)
    {
        return InvokeViewViewModelActionAsync<IAsyncDisposable>(page, x => x.DisposeAsync().AsTask());
    }
}

using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        InvokeViewViewModelAction<IDestructible>(page, x => x.Destroy());
    }

    public static Task DestroyAsync(object? page)
    {
        return InvokeViewViewModelActionAsync<IDestructibleAsync>(page, x => x.DestroyAsync());
    }
}

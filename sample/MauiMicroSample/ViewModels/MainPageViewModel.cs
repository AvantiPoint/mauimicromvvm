using System.Windows.Input;
using MauiMicroMvvm;

namespace MauiMicroSample.ViewModels;

public class MainPageViewModel : MauiMicroViewModel
{
    public MainPageViewModel(ViewModelContext context)
        : base(context)
    {
        PressedCommand = new Command(() => Count++);
    }

    public int Count
    {
        get => Get<int>();
        set => Set(value, () => Message = value == 1 ? $"Pressed {value} time" : $"Pressed {value} times");
    }

    public string Message
    {
        get => Get("Click Me");
        set => Set(value);
    }

    public ICommand PressedCommand { get; }

    public override void OnFirstLoad()
    {
        base.OnFirstLoad();
        Console.WriteLine("MainPageViewModel Initialized");
    }

    public override void OnAppearing()
    {
        base.OnAppearing();
        Console.WriteLine("MainPageViewModel OnAppearing");
    }

    public override void OnDisappearing()
    {
        base.OnDisappearing();
        Console.WriteLine("MainPageViewModel OnDisappearing");
    }

    public override void OnResume()
    {
        base.OnResume();
        Console.WriteLine("MainPageViewModel OnResume");
    }

    public override void OnSleep()
    {
        base.OnSleep();
        Console.WriteLine("MainPageViewModel OnSleep");
    }
}
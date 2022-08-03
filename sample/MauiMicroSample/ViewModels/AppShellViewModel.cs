using MauiMicroMvvm;

namespace MauiMicroSample.ViewModels;

public class AppShellViewModel : MauiMicroViewModel
{
    public AppShellViewModel(ViewModelContext context) 
        : base(context)
    {
        Console.WriteLine("AppShellViewModel has been resolved");
    }
}

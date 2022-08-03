using MauiMicroSample.Modules;
using MauiMicroSample.Pages;
using MauiMicroSample.ViewModels;
using Microsoft.Extensions.Logging;

namespace MauiMicroSample;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiMicroMvvm<AppShell>(
                "Resources/Styles/Colors.xaml",
                "Resources/Styles/Styles.xaml")
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.MapView<MainPage, MainPageViewModel>()
            .MapView<MessageDemoPage, MessageDemoPageViewModel>()
            .MapView<MessageDisplay, MessageDisplayViewModel>()
            .MapView<AppShell, AppShellViewModel>();

        builder.Logging.AddConsole();

        return builder.Build();
    }
}

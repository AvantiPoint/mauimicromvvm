using MauiMicroSample.Modules;
using MauiMicroSample.Pages;
using MauiMicroSample.Services;
using MauiMicroSample.ViewModels;
using Microsoft.Extensions.Logging;
using Refit;

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
            .MapView<MauiInfluencersPage, MauiInfluencersViewModel>()
            .MapView<InfluencerDetail, InfluencerDetailViewModel>()
            .MapView<AppShell, AppShellViewModel>()
            .AddSingleton(Connectivity.Current)
            .AddSingleton(_ => new HttpClient
            {
                BaseAddress = new Uri("https://dansiegel.blob.core.windows.net")
            })
            .AddSingleton(_ => RestService.For<IApiClient>(_.GetRequiredService<HttpClient>()));

        Routing.RegisterRoute($"{nameof(MauiInfluencersPage)}/{nameof(InfluencerDetail)}", typeof(InfluencerDetail));

        builder.Logging.AddConsole();

        return builder.Build();
    }
}

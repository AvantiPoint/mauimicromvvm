using Microsoft.Extensions.Logging;

namespace MauiMicroMvvm;

public class ViewModelContext
{
    public ViewModelContext(ILoggerFactory logger, INavigation navigation, IPageDialogs pageDialogs)
    {
        Logger = logger;
        Navigation = navigation;
        PageDialogs = pageDialogs;
    }

    public ILoggerFactory Logger { get; }
    public INavigation Navigation { get; }
    public IPageDialogs PageDialogs { get; }
}
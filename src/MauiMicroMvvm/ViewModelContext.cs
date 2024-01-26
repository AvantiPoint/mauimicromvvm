using Microsoft.Extensions.Logging;

namespace MauiMicroMvvm;

public class ViewModelContext(ILoggerFactory logger, INavigation navigation, IPageDialogs pageDialogs)
{
    public ILoggerFactory Logger { get; } = logger;
    public INavigation Navigation { get; } = navigation;
    public IPageDialogs PageDialogs { get; } = pageDialogs;
}
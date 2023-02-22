using MauiMicroMvvm;
using MauiMicroSample.Models;
using MauiMicroSample.Services;

namespace MauiMicroSample.ViewModels;

public class MauiInfluencersViewModel : MauiMicroViewModel
{
    private readonly IApiClient _apiClient;
    private readonly IConnectivity _connectiviy;
    public MauiInfluencersViewModel(ViewModelContext context, IApiClient apiClient, IConnectivity connectivity) 
        : base(context)
    {
        _apiClient = apiClient;
        _connectiviy = connectivity;
        RowSelected = new Command<MauiInfluencer>(OnRowSelected);
    }

    public IEnumerable<MauiInfluencer> Influencers
    {
        get => Get<IEnumerable<MauiInfluencer>>();
        set => Set(value);
    }

    public Command<MauiInfluencer> RowSelected { get; }

    public override async void OnFirstLoad()
    {
        IsBusy = true;
        try
        {
            if(_connectiviy.NetworkAccess != NetworkAccess.Internet)
            {
                await PageDialogs.DisplayAlert("Offline", "You're currently offline. Please connect to the internet", "Ok");
                return;
            }
            using var apiResponse = await _apiClient.GetInfluencers();
            if (!apiResponse.IsSuccessStatusCode)
            {
                await PageDialogs.DisplayAlert("Whoops", "We were not able to retrieve the Maui Influencers", "Ok");
                return;
            }

            Influencers = apiResponse.Content;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async void OnRowSelected(MauiInfluencer influencer)
    {
        await Navigation.GoToAsync("InfluencerDetail", new Dictionary<string, object> { { "influencer", influencer } });
    }
}

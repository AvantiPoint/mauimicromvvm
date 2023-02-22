using MauiMicroMvvm;
using MauiMicroSample.Models;

namespace MauiMicroSample.ViewModels;

public class InfluencerDetailViewModel : MauiMicroViewModel
{
    public InfluencerDetailViewModel(ViewModelContext context) 
        : base(context)
    {
    }

    public MauiInfluencer Influencer
    {
        get => Get<MauiInfluencer>();
        set => Set(value);
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
    }
}

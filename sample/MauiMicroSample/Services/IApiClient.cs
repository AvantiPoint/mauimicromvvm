using MauiMicroSample.Models;
using Refit;

namespace MauiMicroSample.Services;

public interface IApiClient
{
    [Get("/media/maui-devs.json")]
    Task<ApiResponse<IEnumerable<MauiInfluencer>>> GetInfluencers();
}

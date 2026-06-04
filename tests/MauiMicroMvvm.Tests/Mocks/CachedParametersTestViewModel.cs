using MauiMicroMvvm;

namespace MauiMicroMvvm.Tests.Mocks;

internal class CachedParametersTestViewModel : TestMauiMicroViewModel
{
    public CachedParametersTestViewModel(ViewModelContext context) : base(context)
    {
    }

    public IQueryParameterMap GetQueryParameterMapForTest()
    {
        return GetQueryParameterMap();
    }
}

using MauiMicroMvvm;

namespace MauiMicroMvvm.Tests.Mocks;

internal class CachedPropertiesTestViewModel : TestMauiMicroViewModel
{
    public CachedPropertiesTestViewModel(ViewModelContext context) : base(context)
    {
    }

    public IQueryPropertyMap GetQueryPropertyMapForTest()
    {
        return GetQueryPropertyMap();
    }
}

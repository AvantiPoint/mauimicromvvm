using System.Reflection;

namespace MauiMicroMvvm.Tests.Mocks;

internal class CachedPropertiesTestViewModel : TestMauiMicroViewModel
{
    public CachedPropertiesTestViewModel(ViewModelContext context) : base(context)
    {
    }

    public IReadOnlyDictionary<string, PropertyInfo> GetQueryablePropertiesForTest()
    {
        return GetQueryableProperties();
    }
}

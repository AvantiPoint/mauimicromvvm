using System.Reflection;
using MauiMicroMvvm;

namespace MauiMicroMvvm.Tests.Mocks;

internal class QueryLookupTrackingViewModel : TestMauiMicroViewModel
{
    private readonly IReadOnlyDictionary<string, PropertyInfo> _queryableProperties;

    public QueryLookupTrackingViewModel(ViewModelContext context) : base(context)
    {
        TrackingProperties = new TrackingQueryPropertyDictionary(new Dictionary<string, PropertyInfo>(StringComparer.InvariantCultureIgnoreCase)
        {
            [nameof(TestProperty)] = typeof(QueryLookupTrackingViewModel).GetProperty(nameof(TestProperty))!,
            [nameof(ValueTypeProperty)] = typeof(QueryLookupTrackingViewModel).GetProperty(nameof(ValueTypeProperty))!,
        });
        _queryableProperties = TrackingProperties;
    }

    public TrackingQueryPropertyDictionary TrackingProperties { get; }

    protected override IReadOnlyDictionary<string, PropertyInfo> GetQueryableProperties()
    {
        return _queryableProperties;
    }
}

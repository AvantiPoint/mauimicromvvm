using MauiMicroMvvm;

namespace MauiMicroMvvm.Tests.Mocks;

internal class QueryLookupTrackingViewModel : TestMauiMicroViewModel
{
    private readonly IQueryPropertyMap _queryPropertyMap;

    public QueryLookupTrackingViewModel(ViewModelContext context) : base(context)
    {
        TrackingProperties = new TrackingQueryPropertyMap(new Dictionary<string, IQueryProperty>(StringComparer.InvariantCultureIgnoreCase)
        {
            [nameof(TestProperty)] = new TestQueryProperty(nameof(TestProperty), typeof(string)),
            [nameof(ValueTypeProperty)] = new TestQueryProperty(nameof(ValueTypeProperty), typeof(int)),
        });
        _queryPropertyMap = TrackingProperties;
    }

    public TrackingQueryPropertyMap TrackingProperties { get; }

    protected override IQueryPropertyMap GetQueryPropertyMap()
    {
        return _queryPropertyMap;
    }
}

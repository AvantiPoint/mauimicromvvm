using MauiMicroMvvm;

namespace MauiMicroMvvm.Tests.Mocks;

internal class QueryLookupTrackingViewModel : TestMauiMicroViewModel
{
    private readonly IQueryParameterMap _queryParameterMap;

    public QueryLookupTrackingViewModel(ViewModelContext context) : base(context)
    {
        TrackingParameters = new TrackingQueryParameterMap(new Dictionary<string, IQueryParameterSetter>(StringComparer.InvariantCultureIgnoreCase)
        {
            [nameof(TestProperty)] = new TestQueryParameterSetter(
                nameof(TestProperty),
                typeof(string),
                (target, value) => ((QueryLookupTrackingViewModel)target).TestProperty = (string)value!),
            [nameof(ValueTypeProperty)] = new TestQueryParameterSetter(
                nameof(ValueTypeProperty),
                typeof(int),
                (target, value) => ((QueryLookupTrackingViewModel)target).ValueTypeProperty = Convert.ToInt32(value)),
        });
        _queryParameterMap = TrackingParameters;
    }

    public TrackingQueryParameterMap TrackingParameters { get; }

    protected override IQueryParameterMap CreateQueryParameterMap()
    {
        return _queryParameterMap;
    }
}

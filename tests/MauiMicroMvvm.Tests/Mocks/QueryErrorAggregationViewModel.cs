using MauiMicroMvvm;

namespace MauiMicroMvvm.Tests.Mocks;

internal class QueryErrorAggregationViewModel : TestMauiMicroViewModel
{
    private readonly IQueryParameterMap _queryParameterMap;

    public QueryErrorAggregationViewModel(ViewModelContext context) : base(context)
    {
        TrackingParameters = new TrackingQueryParameterMap(new Dictionary<string, IQueryParameterSetter>(StringComparer.InvariantCultureIgnoreCase)
        {
            [nameof(TestProperty)] = new TestQueryParameterSetter(
                nameof(TestProperty),
                typeof(string),
                (target, value) => ((QueryErrorAggregationViewModel)target).TestProperty = (string)value!),
            ["FirstFailure"] = new TestQueryParameterSetter(
                "FirstFailure",
                typeof(string),
                (_, _) => throw new InvalidOperationException("first failed")),
            ["SecondFailure"] = new TestQueryParameterSetter(
                "SecondFailure",
                typeof(string),
                (_, _) => throw new ArgumentException("second failed")),
        });
        _queryParameterMap = TrackingParameters;
    }

    public TrackingQueryParameterMap TrackingParameters { get; }

    protected override IQueryParameterMap CreateQueryParameterMap()
    {
        return _queryParameterMap;
    }
}

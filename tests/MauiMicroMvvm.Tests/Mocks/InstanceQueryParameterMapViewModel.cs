using MauiMicroMvvm;

namespace MauiMicroMvvm.Tests.Mocks;

internal class InstanceQueryParameterMapViewModel : TestMauiMicroViewModel
{
    public InstanceQueryParameterMapViewModel(ViewModelContext context) : base(context)
    {
    }

    public int CreateQueryParameterMapCallCount { get; private set; }

    public IQueryParameterMap GetQueryParameterMapForTest()
    {
        return GetQueryParameterMap();
    }

    protected override IQueryParameterMap CreateQueryParameterMap()
    {
        CreateQueryParameterMapCallCount++;

        return new TrackingQueryParameterMap(new Dictionary<string, IQueryParameterSetter>(StringComparer.InvariantCultureIgnoreCase)
        {
            [nameof(TestProperty)] = new TestQueryParameterSetter(
                nameof(TestProperty),
                typeof(string),
                (target, value) => ((InstanceQueryParameterMapViewModel)target).TestProperty = (string)value!),
        });
    }
}

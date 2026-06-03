using MauiMicroMvvm;

namespace MauiMicroMvvm.Tests.Mocks;

internal sealed class TestQueryProperty : IQueryProperty
{
    public TestQueryProperty(string name, Type propertyType)
    {
        Name = name;
        PropertyType = propertyType;
    }

    public string Name { get; }

    public Type PropertyType { get; }
}

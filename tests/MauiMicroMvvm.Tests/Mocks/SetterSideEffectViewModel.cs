using MauiMicroMvvm;

namespace MauiMicroMvvm.Tests.Mocks;

internal class SetterSideEffectViewModel : TestMauiMicroViewModel
{
    public SetterSideEffectViewModel(ViewModelContext context) : base(context)
    {
    }

    public string SetterSideEffectProperty
    {
        get => Get<string>();
        set
        {
            SetterCallCount++;
            Set(value);
        }
    }

    public int SetterCallCount { get; private set; }
}

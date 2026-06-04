using MauiMicroMvvm;

namespace MauiMicroMvvm.Tests.Mocks;

internal class TestMauiMicroViewModel : MauiMicroViewModel
{
    public TestMauiMicroViewModel(ViewModelContext context) : base(context)
    {
    }

    public string TestProperty
    {
        get => Get<string>();
        set => Set(value);
    }

    public int? NullableInt
    {
        get => Get<int?>();
        set => Set(value);
    }

    public int ValueTypeProperty
    {
        get => Get<int>();
        set => Set(value);
    }

    public ComplexQueryModel? ComplexProperty
    {
        get => Get<ComplexQueryModel?>();
        set => Set(value);
    }

    public bool OnParametersSetCalled { get; private set; }

    protected override void OnParametersSet()
    {
        OnParametersSetCalled = true;
        base.OnParametersSet();
    }

    public bool OnFirstLoadCalled { get; private set; }
    public bool OnAppearingCalled { get; private set; }
    public bool OnDisappearingCalled { get; private set; }
    public bool OnResumeCalled { get; private set; }
    public bool OnSleepCalled { get; private set; }

    public override void OnFirstLoad()
    {
        OnFirstLoadCalled = true;
        base.OnFirstLoad();
    }

    public override void OnAppearing()
    {
        OnAppearingCalled = true;
        base.OnAppearing();
    }

    public override void OnDisappearing()
    {
        OnDisappearingCalled = true;
        base.OnDisappearing();
    }

    public override void OnResume()
    {
        OnResumeCalled = true;
        base.OnResume();
    }

    public override void OnSleep()
    {
        OnSleepCalled = true;
        base.OnSleep();
    }
}

using System.ComponentModel;
using MauiMicroMvvm.Xaml;

namespace MauiMicroMvvm.Internals;

[EditorBrowsable(EditorBrowsableState.Never)]
public class AppLifecycleBehavior : Behavior
{
    private bool _didAppear;
    private bool _isVisible;
    private Window _window;
    public Page Page { get; set; }

    public BindableObject View { get; set; }

    protected override void OnAttachedTo(BindableObject bindable)
    {
        base.OnAttachedTo(bindable);
        Page.Appearing += OnAppearing;
        Page.Disappearing += OnDisappearing;

        if(View != Page)
        {
            Page.PropertyChanged += OnPagePropertyChanged;
            View.PropertyChanged += OnViewPropertyChanged;
        }

        var shell = Shell.Current;
        if (shell.Parent is Window window)
        {
            _window = window;
            _window.Resumed += OnResumed;
            _window.Stopped += OnStopped;
        }
    }

    protected override void OnDetachingFrom(BindableObject bindable)
    {
        base.OnDetachingFrom(bindable);
        Page.Appearing -= OnAppearing;
        Page.Disappearing -= OnDisappearing;
        Page.PropertyChanged -= OnPagePropertyChanged;
        View.PropertyChanged -= OnViewPropertyChanged;
        if (_window is not null)
        {
            _window.Resumed -= OnResumed;
            _window.Stopped -= OnStopped;
        }
        _window = null;
        Page = null;
    }

    private void OnViewPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != MauiMicro.SharedContextProperty.PropertyName)
            return;

        var value = MauiMicro.GetSharedContext(View);
        if (MauiMicro.GetSharedContext(Page) != value)
            MauiMicro.SetSharedContext(Page, value);
    }

    private void OnPagePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != MauiMicro.SharedContextProperty.PropertyName)
            return;

        var value = MauiMicro.GetSharedContext(Page);
        if(MauiMicro.GetSharedContext(View) != value)
            MauiMicro.SetSharedContext(View, value);
    }

    private void OnResumed(object sender, EventArgs e)
    {
        if (_isVisible && View.BindingContext is IAppLifecycle lifecycle)
            lifecycle.OnResume();
    }

    private void OnStopped(object sender, EventArgs e)
    {
        if (_isVisible && View.BindingContext is IAppLifecycle lifecycle)
            lifecycle.OnSleep();
    }

    private void OnAppearing(object sender, EventArgs e)
    {
        if (!_didAppear && View.BindingContext is IViewModelActivation initialize)
            initialize.OnFirstLoad();
        _didAppear = true;
        if (View.BindingContext is IViewLifecycle lifecycle)
            lifecycle.OnAppearing();
        _isVisible = true;
    }

    private void OnDisappearing(object sender, EventArgs e)
    {
        if (View.BindingContext is IViewLifecycle lifecycle)
            lifecycle.OnDisappearing();

        _isVisible = false;
    }
}

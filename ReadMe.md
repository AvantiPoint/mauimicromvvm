# MauiMicroMvvm

You might be wondering "Why Dan?". After all don't we normally hear you should use Prism? The answer is yes you should use Prism you'll be able to build much better apps and not constantly hit limitations. But for some reason people think they should use Shell... so here's something you could almost call Prism Lite, but instead I call it MauiMicroMvvm. It's a micro MVVM Framework, except it's done correctly.

## Why did I publish this?

I have a real issue with some of the frameworks I see being pushed that don't understand basic tenants of good architecture. Or that seem to screw up the fundamentals of what MVVM is in relation to MAUI. At the end of the day I thought it was valuable to publish this so that people can see the difference between good architecture and bad architecture.

## What are the advantages of using this?

MAUI is built architecturally in a way that you should be following an MVVM Design pattern. MAUI itself is not an MVVM framework and lacks a few basics that you need for a proper separation of concerns. MauiMicro provides what you need an nothing else. This is why you'll notice there isn't an implementation of ICommand, MAUI already has one. If you need one that does something special look at libraries like Prism.Core, Microsoft.Toolkit.Mvvm or  ReactiveUI.

MauiMicro builds on top of Shell for Navigation while exposing an abstraction layer that lets you continue to test your ViewModel without any coupling to the View layer. But it doesn't stop there as MauiMicro doesn't fall into some bad practices that you will see with other MVVM Frameworks in the Maui Ecosystem like:

- You do not have to use the MauiMicroViewModel... you can use any base class you want. If you implement the MauiMicro interfaces you'll continue to get support for the App/Page Lifecycle events.
- You're not limited to Views being Pages... why because if you understand what MVVM means then you know understand a MVVM View is a MAUI VisualElement and not strictly a Page.
- You're not required to make your page inherit from some base class that I made... you can use any base Page type that you want as long as it works with Shell I don't care.
- By default we will try to autowire your Views that you have mapped for navigation but you can disable this if you want.

## Using the framework

First you need to be sure to call the `UseMauiMicroMvvm` extension method on the `MauiApplicationBuilder`. This will add a couple of services to the IServiceCollection that you might need to manage Navigation or use the native dialogs that would normally be called from the Shell or Page, only this is exposed as an abstracted service to maintain the MVVM pattern. Unless you specifically need to override code in the Application, you can simply specify the AppShell and any resource paths like those in the sample below, and MauiMicro will provide and initialize the application for you.

Next you need to Map the Views <--> ViewModels. You can optionally provide a navigation key if you are registering a Page. This mapping will work for ANY VisualElement.

```cs
var builder = MauiApp.CreateBuilder();
builder
    .UseMauiMicroMvvm<AppShell>("Resources/Styles/Colors.xaml", "Resources/Styles/Styles.xaml")
    .ConfigureFonts(fonts =>
    {
        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
    });

builder.Services.MapView<MainPage, MainPageViewModel>();

return builder.Build();
```

### Autowire Your Views

Once Views have been mapped to ViewModels, the Autowire will happen automatically for Pages that are resolved with Dependency Injection. If you want to disable this you can set the `MauiMicro.Autowire` property to false on the View.

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:micro="http://schemas.mauimicromvvm.com/2022/dotnet/maui"
             micro:MauiMicro.Autowire="False"
             x:Class="MauiMicroSample.Pages.MainPage">
```

In the event that you have a type which is created without Dependency Injection, you can still use the Autowire feature by setting the `MauiMicro.Autowire` property to true on the View. This works on ANY VisualElement as long as you have a mapping for the ViewModel.

```xaml
<StackLayout xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:micro="http://schemas.mauimicromvvm.com/2022/dotnet/maui"
             micro:MauiMicro.Autowire="True"
             x:Class="MauiMicroSample.Controls.MyControl">
```

You can additionally choose to provide a ViewModel for your Shell however you will need to set the `MauiMicro.Autowire` property to true on the Shell.

```xaml
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:micro="http://schemas.mauimicromvvm.com/2022/dotnet/maui"
             micro:MauiMicro.Autowire="True"
             x:Class="MauiMicroSample.AppShell">
```

### Sharing Context

While this isn't supported between the Shell and it's children, you may want to share a context between the ViewModel on your Page and the ViewModel on your child Views. This is possible by using the `MauiMicro.SharedContext` property on the Views. To start be sure to set the `MauiMicro.SharedContext` property on your control View.

```xaml
<StackLayout xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:micro="http://schemas.mauimicromvvm.com/2022/dotnet/maui"
             micro:MauiMicro.Autowire="True"
             micro:MauiMicro.SharedContext="{Binding MyProperty}"
             x:Class="MauiMicroSample.Controls.MyControl">
```

Next be sure to set the property on your Page, and add the control somewhere on your page.

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:micro="http://schemas.mauimicromvvm.com/2022/dotnet/maui"
             xmlns:controls="clr-namespace:MauiMicroSample.Controls"
             micro:MauiMicro.SharedContext="{Binding MyProperty}"
             x:Class="MauiMicroSample.Pages.MainPage">
  <controls:MyControl />
</ContentPage>
```

## View Resolution in the Shell

By default .NET MAUI supports Dependency Injection with the DataTemplate XAML Extension, and MauiMicro supports this as well. 

```xaml
<FlyoutItem Title="Main Page"
            Icon="home.png"
            Route="MainPage">
  <Tab>
    <ShellContent ContentTemplate="{DataTemplate pages:MainPage}" />
  </Tab>
</FlyoutItem>
```

For those that want something a little simpler, you can actually skip putting your route on the elements and then following up with the type you would like for the ContentTemplate. You can instead just use the `MauiMicro.Route` property on the ShellContent. This will automatically set the ContentTemplate and ShellContent.Route for you.

```xaml
<FlyoutItem Title="Maui Influencers"
            Icon="people.png">
  <Tab>
    <ShellContent micro:MauiMicro.Route="MauiInfluencersPage" />
  </Tab>
</FlyoutItem>
```

For those who want to type even less, MauiMicro still has this covered with the `MauiMicro.Route` also supporting Tabs as shown here meaning you can just put it on the tab and the ShellContent will be created for you.

```xaml

<FlyoutItem Title="Message Demo"
            Icon="messages.png">
  <Tab micro:MauiMicro.Route="MessageDemoPage" />
</FlyoutItem>
```

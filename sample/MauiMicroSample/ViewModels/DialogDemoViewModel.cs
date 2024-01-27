using System.Windows.Input;
using MauiMicroMvvm;

namespace MauiMicroSample.ViewModels;

public class DialogDemoViewModel : MauiMicroViewModel
{
    public DialogDemoViewModel(ViewModelContext context) 
        : base(context)
    {
        ShowActionSheetCommand = new Command(async () => {
            await context.PageDialogs.DisplayActionSheet("Sample Action Sheet", "Cancel", "Destroy", "Button 1", "Button 2");
        });

        ShowAlertCommand = new Command(async () => {
            await context.PageDialogs.DisplayAlert("Sample Alert", "This is a sample alert", "Accept", "Cancel");
        });
    }

    public ICommand ShowAlertCommand { get; }

    public ICommand ShowActionSheetCommand { get; }
}
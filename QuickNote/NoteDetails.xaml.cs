using CommunityToolkit.Maui.Views;
using QuickNote.Configurations;
using QuickNote.ViewModels;

namespace QuickNote;

public partial class NoteDetails : ContentPage
{
	public NoteDetails(NoteDetailsVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await (BindingContext as NoteDetailsVM).GetNote();
    }

    protected override bool OnBackButtonPressed()
	{
        //(BindingContext as NoteDetailsVM).GoBack();
        //return base.OnBackButtonPressed();
        Dispatcher.Dispatch(async () => await (BindingContext as NoteDetailsVM).GoBack());
        return true;
    }

    private async void NoteOptions_Clicked(object sender, EventArgs e)
    {
        var result = await this.ShowPopupAsync(new NoteOptions());
        if (result is bool boolResult)
        {
            if (boolResult)
                await (BindingContext as NoteDetailsVM).SetReminderNotify();
            else
                (BindingContext as NoteDetailsVM).InitReminderValues();
        }
    }
}
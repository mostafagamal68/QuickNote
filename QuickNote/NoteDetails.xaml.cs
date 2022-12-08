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
		Shared.NoteId = null;
        return base.OnBackButtonPressed();
    }
}
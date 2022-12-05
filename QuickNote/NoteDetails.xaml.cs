using QuickNote.ViewModels;

namespace QuickNote;

public partial class NoteDetails : ContentPage
{
	public NoteDetails(NoteDetailsVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
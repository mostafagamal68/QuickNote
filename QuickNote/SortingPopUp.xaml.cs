using CommunityToolkit.Maui.Views;
using QuickNote.ViewModels;

namespace QuickNote;

public partial class SortingPopUp : Popup
{
	public SortingPopUp()
	{
		InitializeComponent();
	}

	private void Apply_Clicked(object sender, EventArgs e) => Close(true);
    
}
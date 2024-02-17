using CommunityToolkit.Maui.Views;
using QuickNote.ViewModels;

namespace QuickNote;

public partial class SortingPopUp : Popup
{
	public SortingPopUp()
	{
		Size = new Size( DeviceDisplay.MainDisplayInfo.Width/2.75, DeviceDisplay.MainDisplayInfo.Height/6);
		InitializeComponent();
        ResultWhenUserTapsOutsideOfPopup = false;
    }

    private void Apply_Clicked(object sender, EventArgs e) => Close(true);
    
}
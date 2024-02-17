using CommunityToolkit.Maui.Views;
using QuickNote.ViewModels;

namespace QuickNote;

public partial class FilteringPopUp : Popup
{
    public FilteringPopUp()
    {
		Size = new Size( DeviceDisplay.MainDisplayInfo.Width/2.75, DeviceDisplay.MainDisplayInfo.Height/8);
        InitializeComponent();
        ResultWhenUserTapsOutsideOfPopup = false;
    }

    private void Apply_Clicked(object sender, EventArgs e) => Close(true);

}
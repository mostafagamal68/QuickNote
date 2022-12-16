using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using QuickNote.ViewModels;

namespace QuickNote;

public partial class NoteOptions : Popup
{
    public NoteOptions()
	{
		InitializeComponent();
        ResultWhenUserTapsOutsideOfPopup = false;
	}

    private void RepeatToggle_Toggled(object sender, ToggledEventArgs e)
    {
        RepeatType.IsVisible = !RepeatType.IsVisible;
    }

    private void ReminderToggle_Toggled(object sender, ToggledEventArgs e)
    {
        ReminderDate.IsVisible = !ReminderDate.IsVisible;
        ReminderTime.IsVisible = !ReminderTime.IsVisible;
        RepeatLabel.IsVisible = !RepeatLabel.IsVisible;
        RepeatToggle.IsVisible = !RepeatToggle.IsVisible;
        RepeatToggle.IsToggled = false;
        RepeatType.IsVisible = false;
    }

    private async void Apply_Clicked(object sender, EventArgs e)
    {
        if (RepeatType.SelectedIndex == -1 && RepeatToggle.IsToggled)
            await Toast.Make("Please Select Repeat Type").Show();
        else
            Close(true);
    }
}
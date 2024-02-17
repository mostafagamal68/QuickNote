using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using QuickNote.Helpers;
using QuickNote.ViewModels;

namespace QuickNote;

public partial class NoteOptions : Popup
{
    public NoteOptions()
    {
		Size = new Size( DeviceDisplay.MainDisplayInfo.Width/2.75, DeviceDisplay.MainDisplayInfo.Height/8);
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
        //RepeatToggle.IsToggled = false;
        //RepeatType.IsVisible = false;
    }

    private async void Apply_Clicked(object sender, EventArgs e)
    {
        var status = await CheckPermissions.CheckNotificationPermission();

        if (status != PermissionStatus.Granted)
        {
            await Toast.Make("Notification Permission is needed to set a reminder!").Show();
            Close(false);
        }
        else if (RepeatType.SelectedIndex == -1 && RepeatToggle.IsToggled)
            await Toast.Make("Please Select Repeat Type").Show();
        else
            Close(true);
    }
}
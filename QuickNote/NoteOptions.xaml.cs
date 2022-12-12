using CommunityToolkit.Maui.Views;
using QuickNote.ViewModels;

namespace QuickNote;

public partial class NoteOptions : Popup
{
	public NoteOptions()
	{
		InitializeComponent();
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
}
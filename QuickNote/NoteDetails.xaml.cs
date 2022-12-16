using CommunityToolkit.Maui.Views;
using QuickNote.ViewModels;

namespace QuickNote;

public partial class NoteDetails : ContentPage
{
    private readonly NoteDetailsVM _noteDetailsVM;
    public NoteDetails(NoteDetailsVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _noteDetailsVM = vm;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _noteDetailsVM.GetNote();
    }

    protected override bool OnBackButtonPressed()
    {
        Dispatcher.Dispatch(async () => await _noteDetailsVM.Back());
        return true;
    }

    private async void NoteOptions_Clicked(object sender, EventArgs e)
    {
        bool reminderStatus = _noteDetailsVM.IsReminder;
        bool repeatStatus = _noteDetailsVM.IsReminderRepeatly;
        var reminderDate = _noteDetailsVM.ReminderDate;
        var reminderTime = _noteDetailsVM.ReminderTime;
        var repeatType = _noteDetailsVM.RepeatType;
        if (reminderStatus == false)
        {
            _noteDetailsVM.ReminderDate = DateTime.Today;
            _noteDetailsVM.ReminderTime = DateTime.Now.AddMinutes(1) - DateTime.Today;
        }
        var result = await this.ShowPopupAsync(new NoteOptions());
        if (result is bool boolResult)
        {
            if (boolResult)
                await _noteDetailsVM.SetReminderNotify(reminderStatus);
            else
                _noteDetailsVM.InitReminderValues(reminderStatus, repeatStatus, reminderDate, reminderTime, repeatType);
        }
    }
}
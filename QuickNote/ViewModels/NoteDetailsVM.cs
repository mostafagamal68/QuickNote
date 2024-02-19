using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.LocalNotification;
using QuickNote.Configurations;
using QuickNote.Models;

namespace QuickNote.ViewModels
{
    [QueryProperty("Id", "Id")]
    public partial class NoteDetailsVM : ObservableObject
    {
        private readonly Database database;

        public NoteDetailsVM()
        {
            database = new();
            ReminderDate = DateTime.Today;
            ReminderTime = DateTime.Now.AddMinutes(1) - ReminderDate;
        }

        public async Task GetNote()
        {
            if (Id != 0)
            {
                note = await database.GetItemAsync(Id);

                Id = note.Id;
                Title = $"{note.Name} Details";
                Name = note.Name;
                Description = note.Description;
                Date = note.Date.ToString("dd/MM/yyyy");
                Done = note.Done;
                IsReminder = note.IsReminder;
                IsReminderRepeatly = note.IsReminderRepeatly;
                if (note.ReminderDate.HasValue)
                    ReminderDate = note.ReminderDate.Value.Date;
                if (note.ReminderDate.HasValue)
                    ReminderTime = note.ReminderDate.Value.TimeOfDay;
                RepeatType = note.RepeatType;
            }
            else
            {
                Title = "New Note";
            }
        }

        public void InitReminderValues(bool reminderStatus, bool repeatStatus, DateTime? reminderDate, TimeSpan? reminderTime, string repeatType)
        {
            IsReminder = reminderStatus;
            IsReminderRepeatly = repeatStatus;
            if (reminderDate.HasValue)
                ReminderDate = reminderDate.Value.Date;
            if (reminderTime.HasValue)
                ReminderTime = reminderTime.Value;
            RepeatType = repeatType;
        }

        public async Task SetReminderNotify(bool ReminderStatus)
        {
            if(IsReminder != ReminderStatus)
                await Toast.Make(IsReminder ? "Reminder has been set on" : "Reminder has been set off").Show();
        }

        TimeSpan? GetRepeatTime(string selectedRepeatType, DateTime selectedReminderDate)
        {
            if (selectedRepeatType == "Minutly")
                return TimeSpan.FromMinutes(1);
            if (selectedRepeatType == "Hourly")
                return TimeSpan.FromHours(1);
            else if (selectedRepeatType == "Daily")
                return TimeSpan.FromDays(1);
            else if (selectedRepeatType == "Weekly")
                return TimeSpan.FromDays(7);
            else if (selectedRepeatType == "Monthly")
                return selectedReminderDate.AddMonths(1) - selectedReminderDate;
            else if (selectedRepeatType == "yearly")
                return selectedReminderDate.AddYears(1) - selectedReminderDate;
            else
                return null;
        }

        public bool IsValuesChanged() => !(note.Name == Name && note.Description == Description && note.Done == Done && note.IsReminder == IsReminder &&
                (note.ReminderDate.HasValue ? note.ReminderDate.Value.Date == ReminderDate : 1 == 1) &&
                (note.ReminderDate.HasValue ? note.ReminderDate.Value.TimeOfDay == ReminderTime : 1 == 1) &&
                note.IsReminderRepeatly == IsReminderRepeatly && note.RepeatType == RepeatType);


        QuickNoteItem note = new();

        [ObservableProperty]
        int id;

        [ObservableProperty]
        string title;

        [ObservableProperty]
        string name;

        [ObservableProperty]
        string description;

        [ObservableProperty]
        string date;

        [ObservableProperty]
        DateTime reminderDate;

        [ObservableProperty]
        TimeSpan reminderTime;

        [ObservableProperty]
        string repeatType;

        [ObservableProperty]
        bool isReminder;

        [ObservableProperty]
        bool isReminderRepeatly;

        [ObservableProperty]
        bool done;

        [ObservableProperty]
        bool isLoading;

        [RelayCommand]
        async Task Save()
        {
            IsLoading = true;
            if (!string.IsNullOrWhiteSpace(Name))
            {
                var ReminderDateTime = ReminderDate.Add(ReminderTime);
                QuickNoteItem quickNote = new()
                {
                    Id = Id,
                    Name = Name,
                    Description = Description,
                    Date = DateTime.Now,
                    Done = Done,
                    IsReminder = IsReminder,
                    ReminderDate = IsReminder && ReminderDateTime > DateTime.Now ? ReminderDateTime : null,
                    IsReminderRepeatly = IsReminderRepeatly,
                    RepeatType = IsReminderRepeatly ? RepeatType : ""
                };
                await database.SaveItemAsync(quickNote);

                if (quickNote.IsReminder && quickNote.ReminderDate != null)
                {
                    var notification = new NotificationRequest
                    {
                        NotificationId = quickNote.Id,
                        Title = quickNote.Name,
                        Description = quickNote.Description,
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = quickNote.ReminderDate,
                            NotifyRepeatInterval = GetRepeatTime(quickNote.RepeatType, (DateTime)quickNote.ReminderDate),
                            RepeatType = IsReminderRepeatly ? NotificationRepeat.TimeInterval : NotificationRepeat.No
                        }                        
                    };

                    LocalNotificationCenter.Current.Cancel(quickNote.Id);
                    await LocalNotificationCenter.Current.Show(notification);

                }
                else
                {
                    LocalNotificationCenter.Current.Cancel(quickNote.Id);
                }

                await Toast.Make("Saved Successfully!").Show();
                await Shell.Current.GoToAsync("..", true);
            }
            else
                await Shell.Current.DisplayAlert("Title missing!", "Please enter a note title", "OK");
            IsLoading = false;
        }

        [RelayCommand]
        async Task Back()
        {
            if (IsValuesChanged())
            {
                var answer = await Shell.Current.DisplayAlert("Attention", "There are unsaved changes\nAre you sure to leave?", "Yes", "No");
                if (answer)
                    await Shell.Current.GoToAsync("..", true);
            }
            else
                await Shell.Current.GoToAsync("..", true);
        }        
    }
}

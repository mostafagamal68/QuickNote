using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
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
        }

        public async Task GetNote()
        {
            if (Id != 0)
            {
                var note = await database.GetItemAsync(Id);

                Id = note.Id;
                Title = $"{note.Name} Details";
                Name = note.Name;
                Description = note.Description;
                Date = note.Date.ToString("dd/MM/yyyy");
                Done = note.Done;
            }
            else
            {
                Title = "New Note";
            }
        }

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
                    ReminderDate = ReminderDateTime <= DateTime.Now ? null : ReminderDateTime
                };
                await database.SaveItemAsync(quickNote);

                Shared.NoteId = null;

                await Toast.Make("Saved Successfully!").Show();
                await Shell.Current.GoToAsync("..", true);
            }
            else
                await Shell.Current.DisplayAlert("Title missing!", "Please enter a note title", "OK");
            IsLoading = false;
        }

        [RelayCommand]
        static void GoBack()
        {
            Shared.NoteId = null;
            Shell.Current.GoToAsync("..", true);
        }
    }
}

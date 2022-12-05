using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using QuickNote.Configurations;
using QuickNote.Models;

namespace QuickNote.ViewModels
{
    //[QueryProperty("Id", "Id")]
    public partial class NoteDetailsVM : ObservableObject
    {
        private readonly Database database;

        public NoteDetailsVM()
        {
            database = new();
            _ = GetNote();
        }

        async Task GetNote()
        {
            if (Shared.NoteId != null)
            {
                var note = await database.GetItemAsync((int)Shared.NoteId);

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
        int? id;

        [ObservableProperty]
        string title;

        [ObservableProperty]
        string name;

        [ObservableProperty]
        string description;

        [ObservableProperty]
        string date;

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
                QuickNoteItem quickNote = new()
                {
                    Id = Id ?? 0,
                    Name = Name,
                    Description = Description,
                    Date = DateTime.Now,
                    Done = Done
                };
                await database.SaveItemAsync(quickNote);
                Shared.NoteId = null;
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

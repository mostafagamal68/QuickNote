using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickNote.Configurations;
using QuickNote.Models;

namespace QuickNote.ViewModels
{
    public partial class MainPageVM : ObservableObject
    {
        private Database database;

        public MainPageVM()
        {
            Notes = new();
        }

        public async Task Init()
        {
            database = await Database.Instance;
            await GetNotes();
        }

        public async Task GetNotes()
        {
            IsLoading = true;
            if (Notes.Count != 0)
            {
                Notes.Clear();

            }
            var notesList = await database.GetItemsAsync();
            Notes = notesList.Select(s => new QuickNoteItem
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Date = s.Date,
                Done = s.Done
            }).ToList();
            IsLoading = false;
        }

        [ObservableProperty]
        public List<QuickNoteItem> notes;

        [ObservableProperty]
        bool isLoading;

        [ObservableProperty]
        string searchText;

        //[RelayCommand]
        //void Switch()
        //{
        //    Preferences.Set("Theme", "Dark");
        //    ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        //    foreach (ResourceDictionary dictionaries in mergedDictionaries)
        //    {
        //        var primaryFound = dictionaries.TryGetValue("Black", out var primary);
        //        dictionaries["White"] = primary;

        //    }

        //    //if (primaryFound)
        //}

        [RelayCommand]
        async Task Create()
        {
            await Shell.Current.GoToAsync(nameof(NoteDetails));
        }

        [RelayCommand]
        async Task Search()
        {
            IsLoading = true;
            Notes.Clear();
            var notes = await database.SearchItemsAsync(SearchText);
            Notes = notes.Select(s => new QuickNoteItem
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Date = s.Date,
                Done = s.Done
            }).ToList();
            IsLoading = false;
        }

        [RelayCommand]
        async Task Delete(QuickNoteItem note)
        {
            //IsLoading = true;
            //var note = await database.GetItemAsync(note);
            //var answer = await Shell.Current.DisplayAlert("Warning", $"Are you sure to delete this note ({note.Name})", "Yes", "No");
            //if (answer)
            //{
            //}
            await Snackbar.Make($"Confirm to delete {note.Name} in 5 seconds", async () =>
                {
                    IsLoading = true;
                    await database.DeleteItemAsync(note.Id);
                    await GetNotes();
                    IsLoading = false;
                },
                "Yes", TimeSpan.FromSeconds(5), new SnackbarOptions
                {
                    BackgroundColor = Colors.Red,
                    TextColor = Colors.White,
                }).Show();
        }

        [RelayCommand]
        async Task Tap(int? Id)
        {
            Shared.NoteId = Id;
            await Shell.Current.GoToAsync($"{nameof(NoteDetails)}", true, new Dictionary<string, object> { { "Id", Id } });
        }
    }
}

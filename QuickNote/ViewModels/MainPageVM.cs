using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.LocalNotification;
using QuickNote.Configurations;
using QuickNote.Models;
using System.Collections.Generic;

namespace QuickNote.ViewModels
{
    [QueryProperty("Filter", "Filter")]
    public partial class MainPageVM : ObservableObject
    {
        private static Database database;

        public MainPageVM()
        {
            Notes = new();
        }

        public async Task Init()
        {
            database = await Database.Instance;
            await GetNotes();
        }

        async Task GetNotes()
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

        public async Task GetNotesWithFilter()
        {
            if (SelectedFilter != 0)
            {

                IsLoading = true;

                if (Notes.Count != 0)
                    Notes.Clear();

                List<QuickNoteItem> notesList = new();
                if (SelectedFilter == 1)
                    notesList = await database.GetItemsAsync();
                else if (SelectedFilter == 2)
                    notesList = await database.GetItemsDoneAsync();
                else if (SelectedFilter == 3)
                    notesList = await database.GetItemsNotDoneAsync();

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
        }

        public void GetNotesWithSort()
        {
            if (SelectedSortField != 0)
            {
                IsLoading = true;
                List<QuickNoteItem> sorted = new();
                if (SelectedSortField == 1)
                    sorted = SelectedSortType == 1 ? Notes.OrderBy(o => o.Date).ToList() : Notes.OrderByDescending(o => o.Date).ToList();
                else if (SelectedSortField == 2)
                    sorted = SelectedSortType == 1 ? Notes.OrderBy(o => o.Name).ToList() : Notes.OrderByDescending(o => o.Name).ToList();
                else if (SelectedSortField == 3)
                    sorted = SelectedSortType == 1 ? Notes.OrderBy(o => o.Done).ToList() : Notes.OrderByDescending(o => o.Done).ToList();

                Notes.Clear();
                Notes = sorted;

                IsLoading = false;
            }
        }

        //[RelayCommand]
        public async Task Search()
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

        [ObservableProperty]
        public List<QuickNoteItem> notes;

        [ObservableProperty]
        bool isLoading;

        [ObservableProperty]
        string searchText;

        [ObservableProperty]
        int selectedFilter;

        [ObservableProperty]
        int selectedSortField;

        [ObservableProperty]
        int selectedSortType;

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
                    LocalNotificationCenter.Current.Cancel(note.Id);
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

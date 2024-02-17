using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.LocalNotification;
using QuickNote.Configurations;
using QuickNote.Models;
using System.Globalization;

namespace QuickNote.ViewModels
{
    public partial class MainPageVM : ObservableObject
    {
        private static Database database;

        public MainPageVM()
        {
            Notes = [];
            SelectedFilter = "All";
            SelectedSortField = "Date";
            SelectedSortType = "Descending";
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
                Notes.Clear();

            List<QuickNoteItem> notesList = [];
            if ((string)SelectedFilter == "Done")
                notesList = await database.GetItemsDoneAsync();
            else if ((string)SelectedFilter == "Not done yet")
                notesList = await database.GetItemsNotDoneAsync();
            else
                notesList = await database.GetItemsAsync();

            Notes = notesList.Select(s => new QuickNoteItem
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Date = s.Date,
                    Done = s.Done,
                    IsReminder = s.IsReminder
                })
                .ToList();

            if ((string)SelectedSortField == "Name")
                Notes = (string)SelectedSortType == "Ascending" ? Notes.OrderBy(o => o.Name).ToList() : Notes.OrderByDescending(o => o.Name).ToList();
            else if ((string)SelectedSortField == "Done")
                Notes = (string)SelectedSortType == "Ascending" ? Notes.OrderBy(o => o.Done).ToList() : Notes.OrderByDescending(o => o.Done).ToList();
            else
                Notes = (string)SelectedSortType == "Ascending" ? Notes.OrderBy(o => o.Date).ToList() : Notes.OrderByDescending(o => o.Date).ToList();

            CurrentMainPageSettings.SetValues(Convert.ToString(SelectedFilter), Convert.ToString(SelectedSortField), Convert.ToString(SelectedSortType));
            IsLoading = false;
        }

        public async Task GetNotesWithFilter()
        {
            IsLoading = true;

            if (Notes.Count != 0)
                Notes.Clear();

            List<QuickNoteItem> notesList = [];
            if ((string)SelectedFilter == "All")
                notesList = await database.GetItemsAsync();
            else if ((string)SelectedFilter == "Done")
                notesList = await database.GetItemsDoneAsync();
            else if ((string)SelectedFilter == "Not done yet")
                notesList = await database.GetItemsNotDoneAsync();

            Notes = notesList.Select(s => new QuickNoteItem
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Date = s.Date,
                Done = s.Done,
                IsReminder = s.IsReminder
            }).ToList();

            CurrentMainPageSettings.SetValues(Convert.ToString(SelectedFilter), Convert.ToString(SelectedSortField), Convert.ToString(SelectedSortType));
            IsLoading = false;
        }

        public void GetNotesWithSort()
        {
            IsLoading = true;
            List<QuickNoteItem> sorted = [];
            if ((string)SelectedSortField == "Date")
                sorted = (string)SelectedSortType == "Ascending" ? Notes.OrderBy(o => o.Date).ToList() : Notes.OrderByDescending(o => o.Date).ToList();
            else if ((string)SelectedSortField == "Name")
                sorted = (string)SelectedSortType == "Ascending" ? Notes.OrderBy(o => o.Name).ToList() : Notes.OrderByDescending(o => o.Name).ToList();
            else if ((string)SelectedSortField == "Done")
                sorted = (string)SelectedSortType == "Ascending" ? Notes.OrderBy(o => o.Done).ToList() : Notes.OrderByDescending(o => o.Done).ToList();

            Notes.Clear();
            Notes = sorted;

            CurrentMainPageSettings.SetValues(Convert.ToString(SelectedFilter), Convert.ToString(SelectedSortField), Convert.ToString(SelectedSortType));
            IsLoading = false;
        }

        public void InitMainPageSettings()
        {
            var Values = CurrentMainPageSettings.GetValues();
            SelectedFilter = Values[0];
            SelectedSortField = Values[1];
            SelectedSortType = Values[2];
        }

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
                Done = s.Done,
                IsReminder = s.IsReminder
            })
                .ToList();
            IsLoading = false;
        }

        [ObservableProperty]
        public List<QuickNoteItem> notes;

        [ObservableProperty]
        bool isLoading;

        [ObservableProperty]
        string searchText;

        [ObservableProperty]
        object selectedFilter;

        [ObservableProperty]
        object selectedSortField;

        [ObservableProperty]
        object selectedSortType;

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
        async Task Create() => await Shell.Current.GoToAsync(nameof(NoteDetails));


        [RelayCommand]
        async Task Delete(QuickNoteItem note)
        {
            await Snackbar.Make($"Confirm to delete {note.Name} in 5 seconds", async () =>
                {
                    IsLoading = true;
                    await database.DeleteItemAsync(note.Id);
                    LocalNotificationCenter.Current.Cancel(note.Id);
                    await GetNotes();
                    IsLoading = false;
                },
                "Yes",
                TimeSpan.FromSeconds(5),
                new SnackbarOptions
                {
                    BackgroundColor = Colors.Red,
                    TextColor = Colors.White,
                })
                .Show();
        }

        [RelayCommand]
        async Task Tap(int? Id) => await Shell.Current.GoToAsync(nameof(NoteDetails), true, new Dictionary<string, object> { { "Id", Id } });

    }
}

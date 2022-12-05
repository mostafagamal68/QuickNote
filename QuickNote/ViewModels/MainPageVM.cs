using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickNote.Configurations;
using QuickNote.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace QuickNote.ViewModels
{
    public partial class MainPageVM : ObservableObject
    {
        private Database database;

        public MainPageVM()
        {
            Notes = new();
            //_ = Init();
        }

        public async Task Init()
        {
            database = await Database.Instance;
            await GetNotes();
        }

        public async Task GetNotes(bool? firstInit=null)
        {
            IsLoading = true;
            if (Notes.Count != 0)
            {
                Notes.Clear();

            }
            var notesList = await database.GetItemsAsync();
            //Notes.AddRange(notesList);
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
        async Task Delete(int Id)
        {
            IsLoading = true;
            var note = await database.GetItemAsync(Id);
            var answer = await Shell.Current.DisplayAlert("Warning", $"Are you sure to delete this note ({note.Name})", "Yes", "No");
            if (answer)
            {
                IsLoading = true;
                await database.DeleteItemAsync(Id);
                await GetNotes();
            }
            IsLoading = false;
        }

        [RelayCommand]
        async Task Tap(int? Id)
        {
            Shared.NoteId = Id;
            await Shell.Current.GoToAsync(nameof(NoteDetails));
        }
    }
}

using CommunityToolkit.Maui.Views;
using QuickNote.ViewModels;

namespace QuickNote;

public partial class MainPage : ContentPage
{
    private readonly MainPageVM _mainPageVM;
    public MainPage(MainPageVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _mainPageVM = vm;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _mainPageVM.Init();
    }

    private async void FilterButton_Clicked(object sender, EventArgs e)
    {
        var result = await this.ShowPopupAsync(new FilteringPopUp());
        if (result is bool boolResult)
        {
            if (boolResult)
                await _mainPageVM.GetNotes();
            else
                _mainPageVM.InitMainPageSettings();
        }
    }

    private async void SortButton_Clicked(object sender, EventArgs e)
    {
        var result = await this.ShowPopupAsync(new SortingPopUp());
        if (result is bool boolResult)
        {
            if (boolResult)
                _mainPageVM.GetNotesWithSort();
            else
                _mainPageVM.InitMainPageSettings();
        }
    }

    private async void Entry_TextChanged(object sender, TextChangedEventArgs e) => await (BindingContext as MainPageVM).Search();
}


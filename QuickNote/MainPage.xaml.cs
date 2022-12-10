using CommunityToolkit.Maui.Views;
using QuickNote.ViewModels;
using static SQLite.SQLite3;

namespace QuickNote;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await (BindingContext as MainPageVM).Init();
    }

    private async void FilterButton_Clicked(object sender, EventArgs e)
    {
        var result =  await this.ShowPopupAsync(new FilteringPopUp());
        if (result is bool boolResult)
        {
            if(boolResult)
                await (BindingContext as MainPageVM).GetNotesWithFilter();
        }
    }

    private async void SortButton_Clicked(object sender, EventArgs e)
    {
        var result = await this.ShowPopupAsync(new SortingPopUp());
        if (result is bool boolResult)
        {
            if (boolResult)
                (BindingContext as MainPageVM).GetNotesWithSort();
        }
    }

    private async void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        await (BindingContext as MainPageVM).Search();
    }
}


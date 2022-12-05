using QuickNote.ViewModels;

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
}


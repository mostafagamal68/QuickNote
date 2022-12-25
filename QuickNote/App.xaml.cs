namespace QuickNote;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
        Current.RequestedThemeChanged += (s, a) =>
        {
            Shell.Current.DisplayAlert("Theme has been changed", $"Theme set to {Current.RequestedTheme}","Ok");
        };
    }
}

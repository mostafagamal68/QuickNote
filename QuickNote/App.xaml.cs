using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

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
        AppActions.OnAppAction += AppActions_OnAppAction;
    }
    protected override void OnStart()
    {
        if (IsFirstLaunch())
            HandleFirstLaunch();
    }

    private bool IsFirstLaunch()
    {
        // You can use settings or preferences to store whether it's the first launch
        // Here, you might use a key-value pair stored in application settings
        // You could use Xamarin.Essentials Preferences or other storage mechanisms
        return !Preferences.Get("IsFirstLaunch", false);
    }

    private void HandleFirstLaunch()
    {
        // Set the flag to true to indicate that the app has been launched
        Preferences.Set("IsFirstLaunch", true);
        AppInfo.Current.ShowSettingsUI();
        Toast.Make("Please Allow Alarms & Reminders if not allowed at the end of app settings below!", ToastDuration.Long).Show();
    }

    void AppActions_OnAppAction(object sender, AppActionEventArgs e)
    {
        // Don't handle events fired for old application instances
        // and cleanup the old instance's event handler
        if (Application.Current != this && Application.Current is App app)
        {
            AppActions.OnAppAction -= app.AppActions_OnAppAction;
            return;
        }
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Shell.Current.GoToAsync($"//{e.AppAction.Id}");
        });
    }
}

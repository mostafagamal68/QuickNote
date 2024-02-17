using System.Drawing;

namespace QuickNote;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
	}

    private void SetBlack(string colorKey)
    {
        ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        foreach (ResourceDictionary dictionaries in mergedDictionaries)
        {
            var colorFound = dictionaries.TryGetValue(colorKey, out var colorValue);
            if (colorFound) dictionaries["Black"] = colorValue;
        }
    }

    private void System_Clicked(object sender, EventArgs e)
    {        
        SetBlack("Gray950");
        Application.Current.UserAppTheme = AppTheme.Unspecified;
    }

    private void Light_Clicked(object sender, EventArgs e)
    {
        Application.Current.UserAppTheme = AppTheme.Light;
        SetBlack("Gray950");
    }

    private void Dark_Clicked(object sender, EventArgs e)
    {
        SetBlack("Gray950");
        Application.Current.UserAppTheme = AppTheme.Dark;
    }

    private void Amoled_Clicked(object sender, EventArgs e)
    {
        SetBlack("Black");
        Application.Current.UserAppTheme = AppTheme.Dark;
    }
}
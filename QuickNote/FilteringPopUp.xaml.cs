using CommunityToolkit.Maui.Views;
using QuickNote.ViewModels;

namespace QuickNote;

public partial class FilteringPopUp : Popup
{
    public FilteringPopUp()
    {
        InitializeComponent();
    }

    private void Apply_Clicked(object sender, EventArgs e) => Close(true);

}
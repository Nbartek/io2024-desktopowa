using System.Windows.Input;

namespace PocztaDesktop.Views;

public partial class AdminMainPage : ContentPage
{
    public AdminMainPage()
    {
        InitializeComponent();
    }

    private async void OnEmployeesTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("EmployesList");
    }

    private async void OnWarehouseTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("ParcelsPage");
    }

    private async void OnSettingsTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("SettingsPage");
    }
}

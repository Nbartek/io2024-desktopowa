using Microsoft.Maui.ApplicationModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Windows.Input;

namespace PocztaDesktop.Views;

public partial class AdminMainPage : ContentPage
{
    public AdminMainPage()
    {
        InitializeComponent();
        LoadUserInfo();
    }

    private void LoadUserInfo()
    {
        // Pobierz token
        var token = Preferences.Get("AuthToken", string.Empty);

        if (!string.IsNullOrWhiteSpace(token))
        {
            // Odczytaj dane z tokena
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Wyciągnij login i uprawnienia
            var login = jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;
            var permission = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id_uprawnienia")?.Value;

            string role = permission switch
            {
                "1" => "Admin",
                "2" => "Kierownik",
                _ => "Nieznana rola"
            };

            // Wyświetl dane na stronie
            UserLoginLabel.Text = $"Login: {login}";
            UserPermissionLabel.Text = $"Uprawnienia: {role}";
        }
        else
        {
            UserLoginLabel.Text = "Nie znaleziono danych użytkownika.";
        }
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

using Microsoft.Extensions.Logging.Abstractions;
using PocztaDesktop.Model;
using System.Net.Http.Json;

namespace PocztaDesktop.Views;

public partial class AddEmployeeForm : ContentPage
{
	public AddEmployeeForm()
	{
		InitializeComponent();
	}
    private async void SaveEmployeeButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var newEmployee = new Employee
            {
                Imi� = NameEntry.Text,
                Nazwisko = SurnameEntry.Text,
                Adres = AddressEntry.Text,
                NrTelefonu = PhoneEntry.Text,
                Login = LoginEntry.Text,
                Has�o = PasswordEntry.Text,
                IdUprawnienia = int.TryParse(PermissionIdEntry.Text, out var id) ? id : 0
            };

            using (HttpClient _client = new HttpClient())
            {
                var token = Preferences.Get("AuthToken", string.Empty);

                if (string.IsNullOrWhiteSpace(token))
                {
                    await DisplayAlert("B��d", "Nie masz uprawnie�, aby wykona� t� akcj�.", "OK");
                    return;
                }

                // Dodaj nag��wek Authorization
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var apiUrl = "http://localhost:5118/api/Items/add_employee";
                var response = await _client.PostAsJsonAsync(apiUrl, newEmployee);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Sukces", "U�ytkownik zosta� dodany pomy�lnie.", "OK");
                    await Navigation.PopAsync(); // Powr�t do poprzedniej strony
                }
                else
                {
                    await DisplayAlert("B��d", "Nie uda�o si� doda� u�ytkownika. Spr�buj ponownie.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("B��d", $"Wyst�pi� problem: {ex.Message}", "OK");
        }
    }
}
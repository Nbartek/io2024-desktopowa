using PocztaDesktop.Model;
using System.Text.Json;

namespace PocztaDesktop.Views;

public partial class LadunekDetailsPage : ContentPage
{
    private Ladunek _ladunek;
	public LadunekDetailsPage(Ladunek ladunek)
	{
		InitializeComponent();
        _ladunek = ladunek;
        LoadDetails();
    }

    private async void LoadDetails()
    {
        try
        {
            using (HttpClient _client = new HttpClient())
            {
                var token = Preferences.Get("AuthToken", string.Empty);

                if (string.IsNullOrWhiteSpace(token))
                {
                    await DisplayAlert("Error", "You are not authorized to perform this action.", "OK");
                    return;
                }

                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var apiUrl = $"http://localhost:5118/api/Items/show_ladunek/{_ladunek.IdLadunek}";
                var response = await _client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var ladunekDetails = JsonSerializer.Deserialize<LadunekDetailsDto>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    // Wy�wietlanie szczeg��w �adunku na stronie
                    SamochodLabel.Text = ladunekDetails.IdSamochodu.ToString();
                    StatusLabel.Text = ladunekDetails.Status;

                    // Wy�wietlanie pracownik�w
                    //pracownicyListView.ItemsSource = ladunekDetails.Pracownicy;

                    // Wy�wietlanie paczek
                    //paczkiListView.ItemsSource = ladunekDetails.Paczki;
                }
                else
                {
                    await DisplayAlert("B��d", "Nie uda�o si� pobra� szczeg��w �adunku.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("B��d", $"Wyst�pi� problem: {ex.Message}", "OK");
        }
    }
}
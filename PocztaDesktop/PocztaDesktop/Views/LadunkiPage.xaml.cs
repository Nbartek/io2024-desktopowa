using System.Text.Json;
using PocztaDesktop.Model;

namespace PocztaDesktop.Views;

public partial class LadunkiPage : ContentPage
{
	public LadunkiPage()
	{
		InitializeComponent();
	}

    private async void LoadLadunkiButton_Clicked(object sender, EventArgs e)
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

                // Dodaj nag��wek Authorization
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var apiUrl = "http://localhost:5118/api/Items/show_ladunki"; // Zaktualizowany URL API
                var response = await _client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var ladunki = JsonSerializer.Deserialize<List<Ladunek>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    ladunkiListView.ItemsSource = ladunki;
                }
                else
                {
                    await DisplayAlert("B��d", "Nie uda�o si� pobra� danych z serwera.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("B��d", $"Wyst�pi� problem: {ex.Message}", "OK");
        }
    }


    private async void OnLadunekSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Ladunek ladunek)
        {
            await Navigation.PushAsync(new LadunekDetailsPage(ladunek));
        }
    }
}
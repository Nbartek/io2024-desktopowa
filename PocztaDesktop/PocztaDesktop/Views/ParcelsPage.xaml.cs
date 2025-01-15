using System.Collections.ObjectModel;
using System.Text.Json;
using PocztaDesktop.Model;

namespace PocztaDesktop.Views;

public partial class ParcelsPage : ContentPage
{
    public ObservableCollection<Paczki> Parcels { get; set; }
    public ParcelsPage()
	{
		InitializeComponent();
        Parcels = new ObservableCollection<Paczki>();
        ParcelsCollectionView.ItemsSource = Parcels; // Przypisujemy �r�d�o danych
    }
    private async void LoadParcelsButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            using (HttpClient _client = new HttpClient())
            {
                var apiUrl = "http://localhost:5118/api/Items/show_parcels";
                var response = await _client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    //var parcels = JsonSerializer.Deserialize<List<Paczki>>(json);
                    var parcels = JsonSerializer.Deserialize<List<Paczki>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    Parcels.Clear();
                    foreach (var parcel in parcels)
                    {
                        Parcels.Add(parcel);
                    }

                    await DisplayAlert("Sukces", "Paczki zosta�y za�adowane.", "OK");
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
}
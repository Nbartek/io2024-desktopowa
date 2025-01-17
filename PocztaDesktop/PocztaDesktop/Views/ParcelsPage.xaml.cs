using System.Collections.ObjectModel;
using System.Text.Json;
using PocztaDesktop.Model;

namespace PocztaDesktop.Views;

public partial class ParcelsPage : ContentPage
{
    public ObservableCollection<Paczki> Parcels { get; set; }
    public ObservableCollection<Paczki> FilteredParcels { get; set; }

    public ParcelsPage()
    {
        InitializeComponent();
        Parcels = new ObservableCollection<Paczki>();
        FilteredParcels = new ObservableCollection<Paczki>();
        ParcelsCollectionView.ItemsSource = FilteredParcels; // Przypisujemy Ÿród³o danych z filtrem
    }

    private async void LoadParcelsButton_Clicked(object sender, EventArgs e)
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

                // Dodaj naglowek Authorization
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var apiUrl = "http://localhost:5118/api/Items/show_parcels";
                var response = await _client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var parcels = JsonSerializer.Deserialize<List<Paczki>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    Parcels.Clear();
                    foreach (var parcel in parcels)
                    {
                        Parcels.Add(parcel);
                    }

                    ApplyFilters(); // Filtruj po za³adowaniu
                    await DisplayAlert("Sukces", "Paczki zosta³y za³adowane.", "OK");
                }
                else
                {
                    await DisplayAlert("B³¹d", "Nie uda³o siê pobraæ danych z serwera.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("B³¹d", $"Wyst¹pi³ problem: {ex.Message}", "OK");
        }
    }

    private void ApplyFilters()
    {
        FilteredParcels.Clear();

        foreach (var parcel in Parcels)
        {
            if (FilterGabaryt.IsChecked && !(parcel.Gabaryt ?? false))
                continue;
            if (FilterCzyZniszczona.IsChecked && !(parcel.CzyZniszczona ?? false))
                continue;
            if (FilterDataNadania.IsChecked && !parcel.DataNadania.HasValue)
                continue;

            FilteredParcels.Add(parcel);
        }
    }

    private void Filter_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        ApplyFilters(); // Aktualizuj widok po zmianie stanu checkboxa
    }
}
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using PocztaDesktop.Model;

namespace PocztaDesktop.Views;

public partial class ParcelsPage : ContentPage
{
    private DateTime? _selectedDate;
    public DateTime? SelectedDate
    {
        get => _selectedDate;
        set
        {
            _selectedDate = value;
            ApplyFilters(); // Aktualizuj filtrowane dane
        }
    }
    public ObservableCollection<Paczki> Parcels { get; set; }
    public ObservableCollection<Paczki> FilteredParcels { get; set; }
    public ICommand DeleteParcelCommand { get; }



    public ParcelsPage()
    {
        InitializeComponent();
        BindingContext = this;

        Parcels = new ObservableCollection<Paczki>();
        FilteredParcels = new ObservableCollection<Paczki>();
        ParcelsCollectionView.ItemsSource = FilteredParcels; // Przypisujemy �r�d�o danych z filtrem
        SelectedDate = DateTime.Today;

        //DeleteParcelCommand = new Command<int>(async (id) => await DeleteParcel(id));
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

                    ApplyFilters(); // Filtruj po za�adowaniu
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

    private async Task DeletePackageAsync(int packageId)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                var token = Preferences.Get("AuthToken", string.Empty);
                if (string.IsNullOrWhiteSpace(token))
                {
                    await DisplayAlert("B��d", "Nie jeste� zalogowany.", "OK");
                    return;
                }

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var apiUrl = $"http://localhost:5118/api/Items/delete_parcel/{packageId}"; // Tw�j URL API do usuwania paczki
                var response = await client.DeleteAsync(apiUrl); // Wykonaj ��danie HTTP DELETE

                if (response.IsSuccessStatusCode)
                {
                    // Je�li operacja si� powiedzie, zaktualizuj widok
                    var deletedPackage = Parcels.FirstOrDefault(p => p.IdPaczki == packageId);
                    if (deletedPackage != null)
                    {
                        Parcels.Remove(deletedPackage);
                        FilteredParcels.Remove(deletedPackage);
                    }
                    await DisplayAlert("Sukces", "Paczka zosta�a usuni�ta.", "OK");
                }
                else
                {
                    await DisplayAlert("B��d", "Nie uda�o si� usun�� paczki.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("B��d", $"Wyst�pi� problem: {ex.Message}", "OK");
        }
    }

    private async void OnDeletePackageClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var packageId = (int)button.CommandParameter; // Pobieramy ID paczki z CommandParameter

        bool isConfirmed = await DisplayAlert("Potwierdzenie", "Czy na pewno chcesz usun�� t� paczk�?", "Tak", "Nie");
        if (!isConfirmed)
            return;

        // Wywo�aj metod� usuwania paczki z bazy
        await DeletePackageAsync(packageId);
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
            if (SelectedDate.HasValue && (parcel.DataNadania?.Year != SelectedDate.Value.Year || parcel.DataNadania?.Month != SelectedDate.Value.Month || parcel.DataNadania?.Day != SelectedDate.Value.Day))
                continue;

            FilteredParcels.Add(parcel);
        }
    }

    private void Filter_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        ApplyFilters(); // Aktualizuj widok po zmianie stanu checkboxa
    }
}
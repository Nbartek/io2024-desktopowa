using PocztaDesktop.Model;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace PocztaDesktop.Views;

public partial class EmployesList : ContentPage
{
    public ObservableCollection<Employee> Employees { get; set; }

    public EmployesList()
    {
        InitializeComponent();
        Employees = new ObservableCollection<Employee>();
        EmployeeCollectionView.ItemsSource = Employees;
    }

    private async void LoadEmployeesButton_Clicked(object sender, EventArgs e)
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

                var apiUrl = "http://localhost:5118/api/Items/show_users";
                var response = await _client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var employees = JsonSerializer.Deserialize<List<Employee>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    Employees.Clear();
                    foreach (var employee in employees)
                    {
                        Employees.Add(employee);
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Failed to fetch data from the server.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("AddEmployeeForm");
    }
}
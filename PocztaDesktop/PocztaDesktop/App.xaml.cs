using Microsoft.Maui.Controls;

namespace PocztaDesktop
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            SetTheme("LightTheme");
            MainPage = new AppShell();
        }
        public void SetTheme(string themeKey)
        {
            try
            {
                if (themeKey == "DarkTheme")
                {
                    Resources["BackgroundColor"] = Resources["DarkBackgroundColor"];
                    Resources["TextColor"] = Resources["DarkTextColor"];
                }
                else
                {
                    Resources["BackgroundColor"] = Resources["LightBackgroundColor"];
                    Resources["TextColor"] = Resources["LightTextColor"];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting theme: {ex.Message}");
            }
        }
    }
}

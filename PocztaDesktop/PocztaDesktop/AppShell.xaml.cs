using PocztaDesktop.Views;

namespace PocztaDesktop
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ParcelsPage), typeof(ParcelsPage));
            Routing.RegisterRoute(nameof(AdminMainPage), typeof(AdminMainPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        }
    }
}

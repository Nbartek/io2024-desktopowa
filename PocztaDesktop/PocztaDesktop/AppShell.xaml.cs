using PocztaDesktop.Views;

namespace PocztaDesktop
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ParcelsPage), typeof(ParcelsPage));
        }
    }
}

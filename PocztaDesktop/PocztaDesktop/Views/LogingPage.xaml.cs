namespace PocztaDesktop.Views;
using PocztaDesktop.EndPoint;
using PocztaDesktop.ViewModel;

public partial class LogingPage : ContentPage
{
	public LogingPage()
	{
		InitializeComponent();
        BindingContext = new MainViewModel();
    }
}
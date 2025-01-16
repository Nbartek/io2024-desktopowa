namespace PocztaDesktop.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
	}
    private void Button_Clicked(object sender, EventArgs e)
    {
        this.BackgroundColor = Colors.Blue;
    }
    private void OnResetBackground(object sender, EventArgs e)
    {
        this.BackgroundColor = Colors.White;
    }

}
namespace JourneyForge;

public partial class AppShell : Shell
{
	public AppShell()
	{
        Routing.RegisterRoute("DetailsPage", typeof(DetailsPage));
        InitializeComponent();
	}
}

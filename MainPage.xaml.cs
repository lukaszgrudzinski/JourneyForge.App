using Shared;

namespace JourneyForge;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel viewModel)
	{
		BindingContext = viewModel;

		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as MainViewModel).Init();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync($"//DetailsPage?Quests={((sender as View).BindingContext as Gothic3QuestArea).Name}");
    }
}


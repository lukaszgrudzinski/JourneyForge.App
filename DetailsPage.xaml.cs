using Shared;

namespace JourneyForge;

[QueryProperty(nameof(Quests), "Quests")]
public partial class DetailsPage : ContentPage
{
    string quests;
    public string Quests
    {
        get => quests;
        set
        {
            quests = value;
            OnPropertyChanged();
            (BindingContext as DetailsViewModel).SetQuestArea(value);
        }
    }
    public DetailsPage(DetailsViewModel viewModel)
    {
        BindingContext = viewModel;

        InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//MainPage");
    }
}


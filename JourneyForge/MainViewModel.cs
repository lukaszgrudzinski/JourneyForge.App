using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JourneyForge.Services;
using Shared;

namespace JourneyForge;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private IEnumerable<Gothic3QuestArea> _quests;
    private readonly IFetchQuestsService fetchQuestsService;

    [RelayCommand]
    private void GoToDetails(Gothic3QuestArea questArea)
    {
        
    }

    public MainViewModel(IFetchQuestsService fetchQuestsService)
    {
        this.fetchQuestsService = fetchQuestsService;
        // Task.Run(async () => Quests = await fetchQuestsService.Gothic3QuestAreasAsync());
    }

    public void Init()
    {
        Task.Run(async () => Quests = await fetchQuestsService.Gothic3QuestAreasAsync());

    }
}

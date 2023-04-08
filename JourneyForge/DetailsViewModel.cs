using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JourneyForge.Services;
using Shared;

namespace JourneyForge;

public partial class DetailsViewModel : ObservableObject
{
    private readonly IFetchQuestsService _fetchQuestsService;

    [ObservableProperty]
    private Gothic3QuestArea _selectedGothic3QuestArea;

    public DetailsViewModel(IFetchQuestsService fetchQuestsService)
    {
        _fetchQuestsService = fetchQuestsService;
    }

    public async void SetQuestArea(string name)
    {
        var quests = await _fetchQuestsService.Gothic3QuestAreasAsync();
        SelectedGothic3QuestArea = quests.First(a => a.Name == name);
    }

    [RelayCommand]
    private async Task MarkAsCompleted(Gothic3Quest quest)
    {
        quest.IsCompleted = true;

        await _fetchQuestsService.Save(SelectedGothic3QuestArea);
    }

    [RelayCommand]
    private Task SeeQuestDetails(Gothic3Quest quest)
    {
        return Task.CompletedTask;
    }
}

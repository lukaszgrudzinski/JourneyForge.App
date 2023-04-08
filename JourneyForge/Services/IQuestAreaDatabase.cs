using Shared;

namespace JourneyForge.Services;

internal interface IQuestAreaDatabase
{
    Task<List<Gothic3QuestArea>> GetItemsAsync();
    Task<int> SaveItemAsync(Gothic3QuestArea item);
    Task<bool> IsDataAlreadyThereAsync();
}

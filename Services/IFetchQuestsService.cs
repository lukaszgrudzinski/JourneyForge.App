using Shared;

namespace JourneyForge.Services;

public interface IFetchQuestsService
{
    Task<IReadOnlyList<Gothic3QuestArea>> Gothic3QuestAreasAsync();
    Task Save(Gothic3QuestArea questArea);
}

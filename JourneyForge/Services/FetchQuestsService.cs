using Shared;
using SQLite;
using System.Text.Json;

namespace JourneyForge.Services;

internal class FetchQuestsService : IFetchQuestsService
{
    private readonly IQuestAreaDatabase questAreaDatabase;

    public async Task<IReadOnlyList<Gothic3QuestArea>> Gothic3QuestAreasAsync()
    {
        try
        {
            string fileName = "gothic3quests.json";
            string fileNameLocal = "gothic3questsLocal.json";

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileNameLocal);

            if (File.Exists(path))
            {
                string contents = File.ReadAllText(path);

                return JsonSerializer.Deserialize<IReadOnlyList<Gothic3QuestArea>>(contents);
            }

            using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);

            return await JsonSerializer.DeserializeAsync<IReadOnlyList<Gothic3QuestArea>>(stream);

        }
        catch(Exception ex)
        {
            return null;
        }
    }

    public async Task Save(Gothic3QuestArea questArea)
    {
        var old = await Gothic3QuestAreasAsync();
        var newList = old.ToList();
        int index = newList.FindIndex(area => area.Name == questArea.Name);
        if(index != -1)
        {
            newList[index] = questArea;

            string data = JsonSerializer.Serialize(newList);

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "gothic3questsLocal.json");

            File.WriteAllText(path, data);
        }
    }

    public FetchQuestsService(IQuestAreaDatabase questAreaDatabase)
    {
        this.questAreaDatabase = questAreaDatabase;
    }
}

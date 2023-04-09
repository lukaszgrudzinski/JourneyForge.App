using SQLite;

namespace Shared;

public class Gothic3QuestArea
{
    [PrimaryKey, AutoIncrement, Column("id")]
    public int ID { get; set; }
    public string Name { get; set; } = "Empty name";
    public IEnumerable<Gothic3Quest> Quests { get; set; } = Enumerable.Empty<Gothic3Quest>();
    public IEnumerable<Gothic3Quest> OrderedQuests => Quests
        .OrderByDescending(quest => quest.ReputationType == Gothic3Reputation.City)
        .ThenByDescending(quest => quest.ReputationValue)
        .ThenBy(quest => quest.Title);
    public int TotalExp => Quests.Sum(quest => quest.Exp);
    public int CurrentExp => Quests.Where(quest => quest.IsCompleted).Sum(quest => quest.Exp);
    public int TotalQuests => Quests.Count();
    public int CompletedQuests => Quests.Count(quest => quest.IsCompleted);
    public bool IsCompleted => Quests.Where(quest => quest.ReputationType == Gothic3Reputation.City).All(quest => quest.IsCompleted);
}

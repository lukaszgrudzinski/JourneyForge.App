using Shared;
using SQLite;

namespace JourneyForge.Services
{
    internal class QuestAreaDatabase : IQuestAreaDatabase
    {
        SQLiteAsyncConnection Database;

        public QuestAreaDatabase()
        {
        }

        public async Task<bool> IsDataAlreadyThereAsync()
        {
            await Init();

            return await Database.Table<Gothic3QuestArea>().CountAsync() > 0;

        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await Database.CreateTableAsync<Gothic3QuestArea>();
        }

        public async Task<int> SaveItemAsync(Gothic3QuestArea item)
        {
            await Init();
            if (item.ID != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<List<Gothic3QuestArea>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<Gothic3QuestArea>().ToListAsync();
        }
    }
}

using QuickNote.Models;
using SQLite;

namespace QuickNote.Configurations
{
    public class Database
    {
        static SQLiteAsyncConnection DB;

        public static readonly AsyncLazy<Database> Instance =
            new(async () =>
            {
                var instance = new Database();
                try
                {
                    CreateTableResult result = await DB.CreateTableAsync<QuickNoteItem>();
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                }
                return instance;
            });

        public Database()
        {
            DB = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        async Task Init()
        {
            if (DB is not null)
                return;

            DB = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await DB.CreateTableAsync<QuickNoteItem>();
        }

        public async Task<List<QuickNoteItem>> GetItemsAsync()
        {
            return await DB.Table<QuickNoteItem>().ToListAsync();
        }

        public async Task<List<QuickNoteItem>> SearchItemsAsync(string text)
        {
            return await DB.Table<QuickNoteItem>().Where(w => w.Name.ToLower().Contains(text.ToLower())).ToListAsync();
        }

        public async Task<List<QuickNoteItem>> GetItemsDoneAsync()
        {
            return await DB.Table<QuickNoteItem>().Where(t => t.Done == true).ToListAsync();
        }

        public async Task<List<QuickNoteItem>> GetItemsNotDoneAsync()
        {
            return await DB.Table<QuickNoteItem>().Where(t => t.Done == false).ToListAsync();
        }

        public async Task<QuickNoteItem> GetItemAsync(int id)
        {
            return await DB.Table<QuickNoteItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(QuickNoteItem item)
        {
            if (item.Id != 0)
                return await DB.UpdateAsync(item);
            else
                return await DB.InsertAsync(item);
        }

        public async Task DeleteItemAsync(int Id)
        {
            await DB.DeleteAsync<QuickNoteItem>(Id);
        }
    }
}

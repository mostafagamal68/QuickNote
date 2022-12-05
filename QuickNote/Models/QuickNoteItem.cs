using SQLite;

namespace QuickNote.Models
{
    public class QuickNoteItem
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public bool Done { get; set; }
    }
}

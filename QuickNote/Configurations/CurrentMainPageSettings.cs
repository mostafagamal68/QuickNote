namespace QuickNote.Configurations
{
    public class CurrentMainPageSettings
    {
        private static string FilterType;
        private static string SortField;
        private static string SortType;

        public static void SetValues(string filterType, string sortField, string sortType)
        {
            FilterType = filterType;
            SortField = sortField;
            SortType = sortType;
        }

        public static string[] GetValues() => new string[3] { FilterType, SortField, SortType };       
        
    }

}

namespace QuickNote.Configurations
{
    public class CurrentMainPageSettings
    {
        private static int FilterType;
        private static int SortField;
        private static int SortType;

        public static void SetValues(int filterType, int sortField, int sortType)
        {
            FilterType = filterType;
            SortField = sortField;
            SortType = sortType;
        }

        public static int[] GetValues() => new int[3] { FilterType, SortField, SortType };       
        
    }

}

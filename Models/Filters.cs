namespace tod.Models
{
    public class Filters 
    {
        public Filters(string filterstring) {
            FilterString = filterstring ?? "all-all-all";
            string[] filters = FilterString.Split('-');
            categoryID = filters[0];
            dueDate = filters[1];
            statusID = filters[2];
        }

        public string FilterString { get; }
        public string categoryID { get; }
        public string dueDate { get; }
        public string statusID { get; }

        public bool HasCategory => categoryID.ToLower() != "all";
        public bool HasDueDate => dueDate.ToLower() != "all";
        public bool HasStatus => statusID.ToLower() != "all";

        public static Dictionary<string, string> dueFilterValues = new Dictionary<string, string> {
            {"future", "Future"},
            {"past", "Past"},
            {"today", "Today"},
    }; 

        public bool IsPast => dueDate.ToLower() == "past";
        public bool IsFuture => dueDate.ToLower() == "future"; 
        public bool IsToday => dueDate.ToLower() == "today";
}       

}
namespace tod.Models
{
    public class Filters 
    {
        public Filters(string filterstring) {
            FilterString = filterstring ?? "all-all-all-all";
            string[] filters = FilterString.Split('-');
            categoryID = filters.Length > 0 ? filters[0] : "all";
            dueDate = filters.Length > 1 ? filters[1] : "all";
            statusID = filters.Length > 2 ? filters[2] : "all";
            dueIn = filters.Length > 3 ? filters[3] : "all";
        }

        public string FilterString { get; }
        public string categoryID { get; }
        public string dueDate { get; }
        public string statusID { get; }
        public string dueIn { get; }

        public bool HasCategory => categoryID.ToLower() != "all";
        public bool HasDueDate => dueDate.ToLower() != "all";
        public bool HasStatus => statusID.ToLower() != "all";
        public bool HasDueIn => dueIn.ToLower() != "all";

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
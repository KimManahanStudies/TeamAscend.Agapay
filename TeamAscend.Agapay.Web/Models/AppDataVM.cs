namespace TeamAscend.Agapay.Web.Models
{
    public class AppDataVM
    {
        public string RequestDate { get; set; }
        public List<Phonebook> Hotlines { get; set; }
        public List<BlogPost> Contents { get; set; }
        public List<MapLocation> Locations { get; set; }
    }
}

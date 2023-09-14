namespace TimeTrackingApp.Domain.Entities
{
    public class Hobbies : BaseEntity
    {
        public string HobbyType { get; set; }
        public int Duration { get; set; }
        public Hobbies(string hobbyType, int duration)
        {
            HobbyType = hobbyType;
            Duration = duration;
        }
    }
}

using TimeTrackingApp.Domain.Enums;

namespace TimeTrackingApp.Domain.Entities
{
    public class Reading : BaseEntity
    {
        public int Pages { get; set; }
        public EReading EReading { get; set; }
        public int Duration { get; set; }
        public Reading(int pages,EReading eReading,int duration)
        {
            Pages = pages;
            EReading = eReading;
            Duration = duration;
        }
    }
}

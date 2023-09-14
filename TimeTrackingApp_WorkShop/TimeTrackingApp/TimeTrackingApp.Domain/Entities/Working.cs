using TimeTrackingApp.Domain.Enums;

namespace TimeTrackingApp.Domain.Entities
{
    public class Working : BaseEntity
    {
        public EWorking EWorking { get; set; }
        public int Duration { get; set; }
        public Working(EWorking eWorking, int duration)
        {
            EWorking = eWorking;
            Duration = duration;
        }
    }
} 

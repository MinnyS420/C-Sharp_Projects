using TimeTrackingApp.Domain.Enums;

namespace TimeTrackingApp.Domain.Entities
{
    public class Exercising : BaseEntity
    {
        public int Duration { get; set; }
        public EExercising ChosenActivityType { get; set; }
        public Exercising(int duration, EExercising chosenActivityType)
        {
            Duration = duration;
            ChosenActivityType = chosenActivityType;
        }
    }
}

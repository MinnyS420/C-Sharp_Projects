namespace TimeTrackingApp.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }
}

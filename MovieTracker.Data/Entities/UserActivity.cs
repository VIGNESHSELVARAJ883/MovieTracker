namespace MovieTracker.Data.Entities
{
    public class UserActivity
    {
        public int ActivityId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public string ActivityType { get; set; }
        public string TargetId { get; set; }
        public string Data { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

namespace MovieTracker.Data.Entities
{
    public class UserToWatch
    {
        public int ToWatchId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public byte? Priority { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}

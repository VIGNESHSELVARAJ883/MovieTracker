namespace MovieTracker.Data.Entities
{
    public class UserReview
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public string ReviewText { get; set; }
        public decimal PersonalRating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

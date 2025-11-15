namespace MovieTracker.Data.Entities
{
    public class UserFavourite
    {
        public int FavouriteId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}

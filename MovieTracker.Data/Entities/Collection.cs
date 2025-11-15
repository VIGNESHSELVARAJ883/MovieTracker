namespace MovieTracker.Data.Entities
{
    public class Collection
    {
        public int CollectionId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<CollectionMovie> CollectionMovies { get; set; }
    }
}

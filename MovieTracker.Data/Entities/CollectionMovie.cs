namespace MovieTracker.Data.Entities
{
    public class CollectionMovie
    {
        public int CollectionMovieId { get; set; }
        public int CollectionId { get; set; }
        public Collection Collection { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public string Note { get; set; }
        public int? Position { get; set; }
        public DateTime AddedAt { get; set; }
    }
}

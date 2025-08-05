namespace MovieTracker.Data.Entities
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string? GenreName { get; set; }

        public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
        public ICollection<TVSeriesGenre> TVSeriesGenres { get; set; } = new List<TVSeriesGenre>();
    }
}

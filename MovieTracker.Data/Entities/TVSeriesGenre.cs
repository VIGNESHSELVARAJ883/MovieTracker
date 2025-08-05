namespace MovieTracker.Data.Entities
{
    public class TVSeriesGenre
    {
        public int TVSeriesId { get; set; }
        public TVSeries TVSeries { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}

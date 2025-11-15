namespace MovieTracker.Data.Dtos
{
    public class MovieListDto
    {
        public int MovieId { get; set; }
        public string? Title { get; set; }
        public string? OriginalTitle { get; set; }
        public string? Overview { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? PosterImageUrl { get; set; }
        public string? BackdropImageUrl { get; set; }
        public decimal? Rating { get; set; }
        public int? VoteCount { get; set; }
        public string? OriginalLanguage { get; set; }   // from Languages.Name
        public decimal? Popularity { get; set; }
        public decimal? Runtime { get; set; }
        public string? Status { get; set; }
        public string? Genres { get; set; }  // aggregated via STRING_AGG

        public bool? IsFavourite { get; set; }
        public bool? IsInWatchlist { get; set; }
        public bool? IsWatched { get; set; }
        public string? Review { get; set; }
    }
}

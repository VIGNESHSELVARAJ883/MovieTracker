using System;

namespace MovieTracker.Data.Dtos
{
    public class TVSeriesListDto
    {
        public int TVSeriesId { get; set; }
        public string? Title { get; set; }
        public string? OriginalTitle { get; set; }
        public string? Overview { get; set; }
        public DateTime? FirstAirDate { get; set; }
        public string? PosterImageUrl { get; set; }
        public string? BackdropImageUrl { get; set; }
        public decimal? Rating { get; set; }
        public int? VoteCount { get; set; }
        public string? OriginalLanguage { get; set; }   // from l.Name AS LanguageName
        public decimal? Popularity { get; set; }
        public decimal? EpisodeRuntime { get; set; }
        public string? Status { get; set; }
        public int? TotalSeasons { get; set; }
        public int? TotalEpisodes { get; set; }
        public string? Genres { get; set; }   // aggregated from STRING_AGG
    }
}

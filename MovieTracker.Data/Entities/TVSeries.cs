using System;
using System.Collections.Generic;

namespace MovieTracker.Data.Entities
{
    public class TVSeries
    {
        public int TVSeriesId { get; set; }
        public string? Title { get; set; }
        public DateTime? FirstAirDate { get; set; }
        public string? OriginalTitle { get; set; }
        public string? Overview { get; set; }
        public string? PosterImageUrl { get; set; }
        public string? BackdropImageUrl { get; set; }
        public decimal? Rating { get; set; }
        public int? VoteCount { get; set; }
        public string? OriginalLanguage { get; set; }
        public string? OriginCountry { get; set; }
        public decimal? Popularity { get; set; }
        public string? HomePage { get; set; }
        public string? ProductionCompanies { get; set; }
        public string? ProductionCountries { get; set; }
        public decimal? EpisodeRuntime { get; set; }
        public string? SpokenLanguages { get; set; }
        public string? Status { get; set; }
        public string? TagLine { get; set; }
        public int? TotalSeasons { get; set; }
        public int? TotalEpisodes { get; set; }

        public ICollection<TVSeriesSeason> Seasons { get; set; } = new List<TVSeriesSeason>();
        public ICollection<TVSeriesGenre> Genres { get; set; } = new List<TVSeriesGenre>();
    }
}

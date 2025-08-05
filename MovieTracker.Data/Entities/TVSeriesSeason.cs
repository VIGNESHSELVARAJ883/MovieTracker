using System;
using System.Collections.Generic;

namespace MovieTracker.Data.Entities
{
    public class TVSeriesSeason
    {
        public int Id { get; set; }
        public int SeriesId { get; set; }
        public TVSeries Series { get; set; }

        public DateTime? AirDate { get; set; }
        public int? EpisodeCount { get; set; }
        public string? Name { get; set; }
        public string? Overview { get; set; }
        public string? PosterImageUrl { get; set; }
        public int? SeasonNumber { get; set; }
        public decimal? Rating { get; set; }

        public ICollection<TVSeriesEpisode> Episodes { get; set; } = new List<TVSeriesEpisode>();
    }
}

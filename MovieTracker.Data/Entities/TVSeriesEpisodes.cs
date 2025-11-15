namespace MovieTracker.Data.Entities
{
    public class TVSeriesEpisode
    {
        public int Id { get; set; }
        public int SeasonId { get; set; }
        public TVSeriesSeason Season { get; set; }

        public int ShowId { get; set; }
        public DateTime? AirDate { get; set; }
        public int? EpisodeNumber { get; set; }
        public string? EpisodeType { get; set; }
        public string? Name { get; set; }
        public string? Overview { get; set; }
        public string? ProductionCode { get; set; }
        public int? Runtime { get; set; }
        public int? SeasonNumber { get; set; }
        public string? StillPath { get; set; }
        public decimal? Rating { get; set; }
        public int? VoteCount { get; set; }
        public string? Crew { get; set; }
        public string? GuestStars { get; set; }
    }
}

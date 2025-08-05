namespace MovieTracker.Data.Dtos
{
    public class TVSeriesDetailsDto
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
        public string? Genres { get; set; }
        public List<SeasonDto> Seasons { get; set; } = new();
    }

    public class SeasonDto
    {
        public int SeasonId { get; set; }
        public int SeriesId { get; set; }
        public DateTime? AirDate { get; set; }
        public int? EpisodeCount { get; set; }
        public string? Name { get; set; }
        public string? Overview { get; set; }
        public string? PosterImageUrl { get; set; }
        public int? SeasonNumber { get; set; }
        public decimal? Rating { get; set; }
        public List<EpisodeDto> Episodes { get; set; } = new();
    }

    public class EpisodeDto
    {
        public int EpisodeId { get; set; }
        public int SeasonId { get; set; }
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

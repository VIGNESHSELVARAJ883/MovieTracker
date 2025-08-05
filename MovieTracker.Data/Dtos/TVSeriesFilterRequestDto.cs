namespace MovieTracker.Data.Dtos
{
    public class TVSeriesFilterRequestDto
    {
        public string? SearchTitle { get; set; }
        public int? FirstAirYear { get; set; }
        public decimal? MinRating { get; set; }
        public decimal? MaxRating { get; set; }
        public string? LanguageCode { get; set; } = "en";
        public int? MinEpisodeRuntime { get; set; }
        public int? MaxEpisodeRuntime { get; set; }
        public decimal? MinPopularity { get; set; }
        public decimal? MaxPopularity { get; set; }
        public int? GenreId { get; set; }
        public string? Status { get; set; }
        public string? SortColumn { get; set; } = "Rating";
        public string? SortOrder { get; set; } = "DESC";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 100;
    }
}

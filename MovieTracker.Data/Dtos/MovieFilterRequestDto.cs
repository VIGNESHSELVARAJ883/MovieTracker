namespace MovieTracker.Data.Dtos
{
    public class MovieFilterRequestDto
    {
        public string? SearchTitle { get; set; }
        public int? ReleaseYear { get; set; }
        public decimal? MinRating { get; set; }
        public decimal? MaxRating { get; set; }
        public string? LanguageCode { get; set; } = "en";
        public int? MinRuntime { get; set; }
        public int? MaxRuntime { get; set; }
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

namespace MovieTracker.Data.Dtos
{
    public class MovieFilterRequestDto
    {
        // Search & filtering
        public string? SearchTitle { get; set; }
        public int? ReleaseYear { get; set; }
        public decimal? MinRating { get; set; }
        public decimal? MaxRating { get; set; }
        public string? LanguageCode { get; set; }
        public int? MinRuntime { get; set; }
        public int? MaxRuntime { get; set; }
        public decimal? MinPopularity { get; set; }
        public decimal? MaxPopularity { get; set; }
        public int? GenreId { get; set; }
        public string? Status { get; set; }

        // User-specific filters
        public int? UserId { get; set; }
        public bool? IsWatchlist { get; set; }
        public bool? IsFavourite { get; set; }
        public bool? IsWatched { get; set; }

        // Sorting & Paging
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 100;
    }
}

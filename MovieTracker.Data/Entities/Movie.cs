using System;
using System.Collections.Generic;

namespace MovieTracker.Data.Entities
{
    public class Movie
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
        public string? OriginalLanguage { get; set; }
        public decimal? Popularity { get; set; }
        public string? HomePage { get; set; }
        public string? ProductionCompanies { get; set; }
        public string? ProductionCountries { get; set; }
        public decimal? Runtime { get; set; }
        public string? SpokenLanguages { get; set; }
        public string? Status { get; set; }
        public string? TagLine { get; set; }

        // Relationships
        public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
    }
}

using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieTracker.Data.Dtos;
using MovieTracker.Data.Entities;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http;

namespace MovieTracker.Data.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly string _connectionString;
        private readonly MovieDbContext _movieDbContext;
        public const string imageBaseUrl = "https://image.tmdb.org/t/p/w500";
        private readonly HttpClient _httpClient;
        public MovieRepository(IConfiguration config, MovieDbContext movieDbContext, HttpClient httpClient)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
            _movieDbContext = movieDbContext;
            _httpClient = httpClient;
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<IEnumerable<MovieListDto>> GetFilteredMoviesAsync(MovieFilterRequestDto filter)
        {
            using var conn = Connection;

            var parameters = new DynamicParameters();
            parameters.Add("@SearchTitle", filter.SearchTitle);
            parameters.Add("@ReleaseYear", filter.ReleaseYear);
            parameters.Add("@MinRating", filter.MinRating);
            parameters.Add("@MaxRating", filter.MaxRating);
            parameters.Add("@LanguageCode", filter.LanguageCode);
            parameters.Add("@MinRuntime", filter.MinRuntime);
            parameters.Add("@MaxRuntime", filter.MaxRuntime);
            parameters.Add("@MinPopularity", filter.MinPopularity);
            parameters.Add("@MaxPopularity", filter.MaxPopularity);
            parameters.Add("@GenreId", filter.GenreId);
            parameters.Add("@Status", filter.Status);

            // New filters for user-specific queries
            parameters.Add("@UserId", filter.UserId);
            parameters.Add("@IsWatchlist", filter.IsWatchlist);
            parameters.Add("@IsFavourite", filter.IsFavourite);
            parameters.Add("@IsWatched", filter.IsWatched);

            parameters.Add("@SortColumn", filter.SortColumn);
            parameters.Add("@SortOrder", filter.SortOrder);
            parameters.Add("@PageNumber", filter.PageNumber);
            parameters.Add("@PageSize", filter.PageSize);

            return await conn.QueryAsync<MovieListDto>(
                "sp_GetFilteredMovies",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }


        public async Task<IEnumerable<Language>> GetAllLanguagesAsync()
        {
            using var conn = Connection;
            return await conn.QueryAsync<Language>("SELECT Code, Name FROM Languages");
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            using var conn = Connection;
            return await conn.QueryAsync<Genre>("SELECT GenreId, GenreName FROM Genres");
        }

        public async Task AddMovieAsync(List<MovieDto> movieDto)
        {
            // Get existing MovieIds from DB
            var existingIds = await _movieDbContext.Movies
                .Select(m => m.MovieId)
                .ToListAsync();

            // Filter out movies that already exist
            var newMoviesDto = movieDto
                .Where(m => !existingIds.Contains(m.Id))
                .ToList();

            if (!newMoviesDto.Any())
                return; // Nothing to insert

            // Prepare Movie entities
            var movies = newMoviesDto.Select(x => new Movie
            {
                MovieId = x.Id,
                Title = x.Title,
                ReleaseDate = DateTime.TryParse(x.Release_Date, out var date) ? date : (DateTime?)null,
                Rating = (decimal)x.Vote_Average,
                Overview = x.Overview,
                Popularity = (decimal)x.Popularity,
                OriginalLanguage = x.Original_Language,
                BackdropImageUrl = string.IsNullOrWhiteSpace(x.Backdrop_Path) ? null : $"{imageBaseUrl}{x.Backdrop_Path}",
                PosterImageUrl = string.IsNullOrWhiteSpace(x.Poster_Path) ? null : $"{imageBaseUrl}{x.Poster_Path}",
                OriginalTitle = x.Original_Title,
                VoteCount = x.Vote_Count,
            }).ToList();

            _movieDbContext.Movies.AddRange(movies);

            // Prepare MovieGenre entities
            var movieGenres = newMoviesDto
                .SelectMany(movie => movie.Genre_Ids.Select(genreId => new MovieGenre
                {
                    MovieId = movie.Id,
                    GenreId = genreId
                }))
                .ToList();

            _movieDbContext.MovieGenres.AddRange(movieGenres);

            // Save changes
            await _movieDbContext.SaveChangesAsync();
        }

        public async Task SyncMovies()
        {
            var movies = await _movieDbContext.Movies
               .Where(m =>
                                (m.HomePage == null && m.ProductionCompanies == null &&
                                 m.ProductionCountries == null && m.SpokenLanguages == null &&
                                 m.Runtime == null && m.TagLine == null)).ToListAsync();


            foreach (var movie in movies)
            {
                var details = await FetchMovieDetailsAsync(movie.MovieId);
                if (details == null) continue;

                movie.Runtime = details.runtime;
                movie.TagLine = details.tagline;
                movie.HomePage = details.homepage;
                movie.Status = details.status;
                movie.ProductionCompanies = JsonConvert.SerializeObject(details.production_companies);
                movie.ProductionCountries = JsonConvert.SerializeObject(details.production_countries);
                movie.SpokenLanguages = JsonConvert.SerializeObject(details.spoken_languages);

                // Insert missing genres from detail API
                if (details.genres != null)
                {
                    var existingGenres = await _movieDbContext.MovieGenres
                        .Where(mg => mg.MovieId == movie.MovieId)
                        .Select(mg => mg.GenreId)
                        .ToListAsync();

                    foreach (var genre in details.genres)
                    {
                        if (!existingGenres.Contains(genre.id))
                        {
                            _movieDbContext.MovieGenres.Add(new MovieGenre
                            {
                                MovieId = movie.MovieId,
                                GenreId = genre.id
                            });
                        }
                    }
                }
            }
        }

        private async Task<TMDBMovieDetails?> FetchMovieDetailsAsync(int movieId)
        {
            var response = await _httpClient.GetAsync($"movie/{movieId}?language=en-US");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TMDBMovieDetails>(json);
        }

    }

}

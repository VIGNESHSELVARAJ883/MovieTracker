using MovieTracker.Data.Dtos;
using MovieTracker.Data.Entities;
using MovieTracker.Data.Repository;
using Newtonsoft.Json;
using Serilog;

namespace MovieTracker.Service
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _repository;
        private readonly HttpClient _httpClient;

        public MovieService(IMovieRepository repository, HttpClient httpClient)
        {
            _repository = repository;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<MovieListDto>> GetFilteredMoviesAsync(MovieFilterRequestDto filter)
        {
            Log.Information("GetFilteredMoviesAsync called at {Time} with filter {@Filter}", DateTime.UtcNow, filter);

            var movies = await _repository.GetFilteredMoviesAsync(filter);

            Log.Information("GetFilteredMoviesAsync returning {Count} movies at {Time}", movies.Count(), DateTime.UtcNow);

            return movies;
        }

        public async Task<IEnumerable<Language>> GetAllLanguagesAsync()
        {
            Log.Information("Fetching all languages at {Time}", DateTime.UtcNow);

            var languages = await _repository.GetAllLanguagesAsync();

            Log.Information("Fetched {Count} languages at {Time}", languages.Count(), DateTime.UtcNow);

            return languages;
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            Log.Information("Fetching all genres at {Time}", DateTime.UtcNow);

            var genres = await _repository.GetAllGenresAsync();

            Log.Information("Fetched {Count} genres at {Time}", genres.Count(), DateTime.UtcNow);

            return genres;
        }

        public async Task AddMovieAsync(List<MovieDto> movie)
        {
            Log.Information("AddMovieAsync called at {Time} for {@Movies}", DateTime.UtcNow, movie);

            await _repository.AddMovieAsync(movie);

            Log.Information("AddMovieAsync completed at {Time}", DateTime.UtcNow);
        }

        public async Task SyncMovies()
        {
            Log.Information("SyncMovies started at {Time}", DateTime.UtcNow);

            var movies = await _repository.GetMoviesNeedingDetailsAsync();
            Log.Information("{Count} movies need details", movies.Count());

            foreach (var movie in movies)
            {
                var details = await FetchMovieDetailsAsync(movie.MovieId);
                if (details == null)
                {
                    Log.Warning("No details found for movie {MovieId}", movie.MovieId);
                    continue;
                }

                movie.Runtime = details.runtime;
                movie.TagLine = details.tagline;
                movie.HomePage = details.homepage;
                movie.Status = details.status;
                movie.ProductionCompanies = JsonConvert.SerializeObject(details.production_companies);
                movie.ProductionCountries = JsonConvert.SerializeObject(details.production_countries);
                movie.SpokenLanguages = JsonConvert.SerializeObject(details.spoken_languages);

                Log.Information("Updating movie {MovieId} with details {@Details}", movie.MovieId, details);

                var genresToAdd = new List<MovieGenre>();
                if (details.genres != null)
                {
                    var existingGenres = await _repository.GetMovieGenreIdsAsync(movie.MovieId);

                    foreach (var genre in details.genres)
                    {
                        if (!existingGenres.Contains(genre.id))
                        {
                            genresToAdd.Add(new MovieGenre
                            {
                                MovieId = movie.MovieId,
                                GenreId = genre.id
                            });
                        }
                    }
                }

                await _repository.UpdateMovieAsync(movie, genresToAdd);

                Log.Information("Movie {MovieId} updated successfully at {Time}", movie.MovieId, DateTime.UtcNow);
            }

            Log.Information("SyncMovies completed at {Time}", DateTime.UtcNow);
        }

        private async Task<TMDBMovieDetails?> FetchMovieDetailsAsync(int movieId)
        {
            Log.Information("Fetching TMDB details for movie {MovieId}", movieId);

            var response = await _httpClient.GetAsync($"movie/{movieId}?language=en-US");
            if (!response.IsSuccessStatusCode)
            {
                Log.Warning("Failed to fetch details for movie {MovieId}, StatusCode: {StatusCode}", movieId, response.StatusCode);
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var details = JsonConvert.DeserializeObject<TMDBMovieDetails>(json);

            Log.Information("Fetched details for movie {MovieId}: {@Details}", movieId, details);

            return details;
        }
    }
}

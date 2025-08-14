using MovieTracker.Data.Dtos;
using MovieTracker.Data.Entities;
using MovieTracker.Data.Repository;

namespace MovieTracker.Service
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _repository;

        public MovieService(IMovieRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MovieListDto>> GetFilteredMoviesAsync(MovieFilterRequestDto filter)
        {
            return await _repository.GetFilteredMoviesAsync(filter);
        }

        public async Task<IEnumerable<Language>> GetAllLanguagesAsync()
        {
            return await _repository.GetAllLanguagesAsync();
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            return await _repository.GetAllGenresAsync();
        }

        public async Task AddMovieAsync(List<MovieDto> movie)
        {
            await _repository.AddMovieAsync(movie);
        }

        public async Task SyncMovies()
        {
            await _repository.SyncMovies();
        }
    }
}

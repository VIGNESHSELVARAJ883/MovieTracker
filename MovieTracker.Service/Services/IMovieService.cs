using MovieTracker.Data.Dtos;
using MovieTracker.Data.Entities;

namespace MovieTracker.Service
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieListDto>> GetFilteredMoviesAsync(MovieFilterRequestDto filter);
        Task<IEnumerable<Language>> GetAllLanguagesAsync();
        Task<IEnumerable<Genre>> GetAllGenresAsync();
    }
}
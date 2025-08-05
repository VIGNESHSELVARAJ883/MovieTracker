using MovieTracker.Data.Dtos;
using MovieTracker.Data.Entities;

namespace MovieTracker.Data.Repository
{
    public interface IMovieRepository
    {
        Task<IEnumerable<MovieListDto>> GetFilteredMoviesAsync(MovieFilterRequestDto filter);
        Task<IEnumerable<Language>> GetAllLanguagesAsync();
        Task<IEnumerable<Genre>> GetAllGenresAsync();
    }
}
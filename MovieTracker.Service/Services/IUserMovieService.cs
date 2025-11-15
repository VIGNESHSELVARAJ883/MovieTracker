using MovieTracker.Data.Dtos;

namespace MovieTracker.Service.Services
{
    public interface IUserMovieService
    {
        Task AddToWatchlistAsync(int userId, int movieId);
        Task RemoveFromWatchlistAsync(int userId, int movieId);

        Task MarkAsWatchedAsync(int userId, int movieId);
        Task RemoveFromWatchedAsync(int userId, int movieId);

        Task AddToFavouritesAsync(int userId, int movieId);
        Task RemoveFromFavouritesAsync(int userId, int movieId);

        Task AddReviewAsync(int userId, int movieId, ReviewDto reviewDto);
        Task RemoveReviewAsync(int userId, int movieId);

        Task<IEnumerable<MovieListDto>> GetWatchedMoviesAsync(int userId);
        Task<IEnumerable<MovieListDto>> GetFavouriteMoviesAsync(int userId);
        Task<IEnumerable<MovieListDto>> GetToBeWatchedMoviesAsync(int userId);
    }

}

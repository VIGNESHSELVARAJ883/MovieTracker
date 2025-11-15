using MovieTracker.Data.Dtos;

namespace MovieTracker.Data.Repositories
{
    public interface IUserMovieRepository
    {
        Task AddToWatchlistAsync(int userId, int movieId);
        Task RemoveFromWatchlistAsync(int userId, int movieId);

        Task MarkAsWatchedAsync(int userId, int movieId);
        Task RemoveFromWatchedAsync(int userId, int movieId);

        Task AddToFavouritesAsync(int userId, int movieId);
        Task RemoveFromFavouritesAsync(int userId, int movieId);

        Task AddReviewAsync(int userId, int movieId, ReviewDto reviewDto);
        Task RemoveReviewAsync(int userId, int movieId);
    }

}

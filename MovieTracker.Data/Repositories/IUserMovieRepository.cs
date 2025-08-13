using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        Task AddReviewAsync(int userId, int movieId, string review,decimal rating);
        Task RemoveReviewAsync(int userId, int movieId);
    }

}

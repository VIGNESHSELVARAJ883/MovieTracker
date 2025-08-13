using MovieTracker.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker.Data.Repositories
{
    public class UserMovieRepository : IUserMovieRepository
    {
        private readonly MovieDbContext _context;

        public UserMovieRepository(MovieDbContext context)
        {
            _context = context;
        }

        public async Task AddToWatchlistAsync(int userId, int movieId)
        {
            if (!_context.UserToWatch.Any(x => x.UserId == userId && x.MovieId == movieId))
            {
                _context.UserToWatch.Add(new UserToWatch { UserId = userId, MovieId = movieId });
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFromWatchlistAsync(int userId, int movieId)
        {
            var item = _context.UserToWatch
                .FirstOrDefault(x => x.UserId == userId && x.MovieId == movieId);
            if (item != null)
            {
                _context.UserToWatch.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAsWatchedAsync(int userId, int movieId)
        {
            if (!_context.UserWatchlist.Any(x => x.UserId == userId && x.MovieId == movieId))
            {
                _context.UserWatchlist.Add(new UserWatchlist { UserId = userId, MovieId = movieId });
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFromWatchedAsync(int userId, int movieId)
        {
            var item = _context.UserWatchlist
                .FirstOrDefault(x => x.UserId == userId && x.MovieId == movieId);
            if (item != null)
            {
                _context.UserWatchlist.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddToFavouritesAsync(int userId, int movieId)
        {
            if (! _context.UserFavourites.Any(x => x.UserId == userId && x.MovieId == movieId))
            {
                _context.UserFavourites.Add(new UserFavourite { UserId = userId, MovieId = movieId });
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFromFavouritesAsync(int userId, int movieId)
        {
            var item = _context.UserFavourites
                .FirstOrDefault(x => x.UserId == userId && x.MovieId == movieId);
            if (item != null)
            {
                _context.UserFavourites.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddReviewAsync(int userId, int movieId, string review, decimal rating)
        {
            var existing = _context.UserReviews
                .FirstOrDefault(x => x.UserId == userId && x.MovieId == movieId);
            if (existing == null)
            {
                _context.UserReviews.Add(new UserReview { UserId = userId, MovieId = movieId, ReviewText = review });
            }
            else
            {
                existing.ReviewText = review;
                existing.PersonalRating = rating;
            }
            await _context.SaveChangesAsync();
        }

        public async Task RemoveReviewAsync(int userId, int movieId)
        {
            var review = _context.UserReviews
                .FirstOrDefault(x => x.UserId == userId && x.MovieId == movieId);
            if (review != null)
            {
                _context.UserReviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }
    }

}

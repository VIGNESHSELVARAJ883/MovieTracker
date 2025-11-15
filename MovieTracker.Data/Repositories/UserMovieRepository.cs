using MovieTracker.Data.Dtos;
using MovieTracker.Data.Entities;

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
            if (!_context.UserFavourites.Any(x => x.UserId == userId && x.MovieId == movieId))
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

        public async Task AddReviewAsync(int userId, int movieId, ReviewDto reviewDto)
        {
            var existing = _context.UserReviews
                .FirstOrDefault(x => x.UserId == userId && x.MovieId == movieId);
            if (existing == null)
            {
                _context.UserReviews.Add(new UserReview { UserId = userId, MovieId = movieId, ReviewText = reviewDto.Review, PersonalRating = reviewDto.Rating });
            }
            else
            {
                existing.ReviewText = reviewDto.Review;
                existing.PersonalRating = reviewDto.Rating;
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

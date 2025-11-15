using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTracker.Data.Dtos;
using MovieTracker.Service.Services;
using System.Security.Claims;

namespace MovieTracker.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserMoviesController : ControllerBase
    {
        private readonly IUserMovieService _userMovieService;

        public UserMoviesController(IUserMovieService userMovieService)
        {
            _userMovieService = userMovieService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());

        // --- Add ---
        [HttpPost("watchlist/{movieId}")]
        public async Task<IActionResult> AddToWatchlist(int movieId)
        {
            await _userMovieService.AddToWatchlistAsync(GetUserId(), movieId);
            return Ok(new { message = "Movie added to watchlist" });
        }

        [HttpPost("watched/{movieId}")]
        public async Task<IActionResult> MarkAsWatched(int movieId)
        {
            await _userMovieService.MarkAsWatchedAsync(GetUserId(), movieId);
            return Ok(new { message = "Movie marked as watched" });
        }

        [HttpPost("favourite/{movieId}")]
        public async Task<IActionResult> AddToFavourites(int movieId)
        {
            await _userMovieService.AddToFavouritesAsync(GetUserId(), movieId);
            return Ok(new { message = "Movie added to favourites" });
        }

        [HttpPost("review/{movieId}")]
        public async Task<IActionResult> AddReview(int movieId, [FromBody] ReviewDto reviewDto)
        {
            await _userMovieService.AddReviewAsync(GetUserId(), movieId, reviewDto);
            return Ok(new { message = "Review added" });
        }

        // --- Remove ---
        [HttpDelete("watchlist/{movieId}")]
        public async Task<IActionResult> RemoveFromWatchlist(int movieId)
        {
            await _userMovieService.RemoveFromWatchlistAsync(GetUserId(), movieId);
            return Ok(new { message = "Movie removed from watchlist" });
        }

        [HttpDelete("watched/{movieId}")]
        public async Task<IActionResult> RemoveFromWatched(int movieId)
        {
            await _userMovieService.RemoveFromWatchedAsync(GetUserId(), movieId);
            return Ok(new { message = "Movie removed from watched list" });
        }

        [HttpDelete("favourite/{movieId}")]
        public async Task<IActionResult> RemoveFromFavourites(int movieId)
        {
            await _userMovieService.RemoveFromFavouritesAsync(GetUserId(), movieId);
            return Ok(new { message = "Movie removed from favourites" });
        }

        [HttpDelete("review/{movieId}")]
        public async Task<IActionResult> RemoveReview(int movieId)
        {
            await _userMovieService.RemoveReviewAsync(GetUserId(), movieId);
            return Ok(new { message = "Review removed" });
        }

        [HttpGet("watched")]
        public async Task<IActionResult> GetWatchedMovies()
        {
            var movies = await _userMovieService.GetWatchedMoviesAsync(GetUserId());
            return Ok(movies);
        }

        [HttpGet("favourite")]
        public async Task<IActionResult> GetFavouriteMovies()
        {
            var movies = await _userMovieService.GetFavouriteMoviesAsync(GetUserId());
            return Ok(movies);
        }

        [HttpGet("watchlist")]
        public async Task<IActionResult> GetToWatchMovies()
        {
            var movies = await _userMovieService.GetToBeWatchedMoviesAsync(GetUserId());
            return Ok(movies);
        }
    }

}

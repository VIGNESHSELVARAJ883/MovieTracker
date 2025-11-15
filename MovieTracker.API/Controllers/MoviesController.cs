using Datadog.Trace; // ✅ Add this namespace for Datadog tracing
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTracker.Data.Dtos;
using MovieTracker.Service;
using System.Security.Claims;

namespace MovieTracker.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());

        [HttpGet("search")]
        public async Task<IActionResult> SearchMoviesAsync([FromQuery] MovieFilterRequestDto filter)
        {
            using (var scope = Tracer.Instance.StartActive("movies.search"))
            {
                var span = scope.Span;
                span.ResourceName = "GET /api/movies/search";
                span.SetTag("endpoint", "SearchMovies");
                span.SetTag("user.id", GetUserId().ToString());

                filter.UserId = GetUserId();
                var movies = await _movieService.GetFilteredMoviesAsync(filter);

                span.SetTag("movies.count", movies.Count().ToString());
                return Ok(movies);
            }
        }

        [HttpGet("languages")]
        public async Task<IActionResult> GetLanguagesAsync()
        {
            using (var scope = Tracer.Instance.StartActive("movies.languages"))
            {
                var span = scope.Span;
                span.ResourceName = "GET /api/movies/languages";
                span.SetTag("endpoint", "GetLanguages");

                var languages = await _movieService.GetAllLanguagesAsync();
                span.SetTag("languages.count", languages.Count().ToString());

                return Ok(languages);
            }
        }

        [HttpGet("genres")]
        public async Task<IActionResult> GetGenresAsync()
        {
            using (var scope = Tracer.Instance.StartActive("movies.genres"))
            {
                var span = scope.Span;
                span.ResourceName = "GET /api/movies/genres";
                span.SetTag("endpoint", "GetGenres");

                var genres = await _movieService.GetAllGenresAsync();
                span.SetTag("genres.count", genres.Count().ToString());

                return Ok(genres);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddMoviesAsync(List<MovieDto> movies)
        {
            using (var scope = Tracer.Instance.StartActive("movies.add"))
            {
                var span = scope.Span;
                span.ResourceName = "POST /api/movies";
                span.SetTag("endpoint", "AddMovies");
                span.SetTag("movies.count", movies?.Count.ToString() ?? "0");

                try
                {
                    await _movieService.AddMovieAsync(movies);
                    span.SetTag("status", "success");
                    return Ok(new { message = "Movies successfully added to the database." });
                }
                catch (Exception ex)
                {
                    span.SetException(ex);
                    throw;
                }
            }
        }

        [HttpGet("sync")]
        public async Task<IActionResult> SyncMovies()
        {
            using (var scope = Tracer.Instance.StartActive("movies.sync"))
            {
                var span = scope.Span;
                span.ResourceName = "GET /api/movies/sync";
                span.SetTag("endpoint", "SyncMovies");

                try
                {
                    await _movieService.SyncMovies();
                    span.SetTag("status", "success");
                    return Ok("Synced successfully.");
                }
                catch (Exception ex)
                {
                    span.SetException(ex);
                    throw;
                }
            }
        }
    }
}

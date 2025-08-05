using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTracker.Data.Dtos;
using MovieTracker.Service;

namespace MovieTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchMoviesAsync([FromQuery] MovieFilterRequestDto filter)
        {
            var movies = await _movieService.GetFilteredMoviesAsync(filter);
            return Ok(movies);
        }
        [HttpGet("languages")]
        public async Task<IActionResult> GetLanguagesAsync()
        {
            var languages = await _movieService.GetAllLanguagesAsync();
            return Ok(languages);
        }

        [HttpGet("genres")]
        public async Task<IActionResult> GetGenresAsync()
        {
            var genres = await _movieService.GetAllGenresAsync();
            return Ok(genres);
        }
    }
}

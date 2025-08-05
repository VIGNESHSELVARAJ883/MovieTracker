//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using MovieTracker.Service;
//using MovieTracker.Service.Models;

//namespace MovieTracker.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TMDBController : ControllerBase
//    {
//        private readonly ITMDBMovieService _tMDBMovieService;

//        public TMDBController(ITMDBMovieService tMDBMovieService)
//        {
//            _tMDBMovieService = tMDBMovieService;
//        }

//        [HttpGet("genre")]
//        public async Task<ActionResult<List<Genere>>> GetGenre()
//        {
//            try
//            {
//                var genres = await _tMDBMovieService.GetGeneres();
//                return Ok(genres.ToList());
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
//            }
//        }

//        [HttpGet("movie")]
//        public async Task<ActionResult<List<Movie>>> GetMovie()
//        {
//            try
//            {
//                var movies = await _tMDBMovieService.GetMovies();
//                return Ok(movies);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
//            }
//        }
//        [HttpPost("movies/add/{pageNumber}")]
//        public async Task<IActionResult> AddMoviesAsync(int pageNumber, [FromQuery] DateTime from, [FromQuery] DateTime to, [FromQuery] int totalPage)
//        {
//            try
//            {
//                if (from > to)
//                {
//                    return BadRequest(new { error = "The 'from' date cannot be later than the 'to' date." });
//                }

//                await _tMDBMovieService.InsertMoviesIntoDb(pageNumber, from, to, totalPage);
//                return Ok(new { message = "Movies successfully added to the database." });
//            }
//            catch (ArgumentException argEx)
//            {
//                // Handle specific argument errors
//                return BadRequest(new { error = argEx.Message });
//            }
//            catch (Exception ex)
//            {
//                // Log the exception (using a logger, if available)
//                // _logger.LogError(ex, "An error occurred while adding movies.");
//                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred. Please try again later.", details = ex.Message });
//            }
//        }

//        [HttpPost("series/add/{pageNumber}")]
//        public async Task<IActionResult> AddSeriesAsync(int pageNumber, [FromQuery] DateTime from, [FromQuery] DateTime to, [FromQuery] int totalPage)
//        {
//            try
//            {
//                if (from > to)
//                {
//                    return BadRequest(new { error = "The 'from' date cannot be later than the 'to' date." });
//                }

//                await _tMDBMovieService.InsertTVSeriesIntoDb(pageNumber, from, to, totalPage);
//                return Ok(new { message = "TVSeries successfully added to the database." });
//            }
//            catch (ArgumentException argEx)
//            {
//                // Handle specific argument errors
//                return BadRequest(new { error = argEx.Message });
//            }
//            catch (Exception ex)
//            {
//                // Log the exception (using a logger, if available)
//                // _logger.LogError(ex, "An error occurred while adding movies.");
//                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred. Please try again later.", details = ex.Message });
//            }
//        }

//        [HttpPost("movies/update/{lastmovieId}")]
//        public async Task<IActionResult> UpdatemoviesAsync(int lastmovieId)
//        {
//            try
//            {

//                var id = await _tMDBMovieService.UpdateMoviesAsync(lastmovieId);
//                return Ok(new { message = "Movies Updated Successfully", lastUpdatedId = id });
//            }
//            catch (ArgumentException argEx)
//            {
//                // Handle specific argument errors
//                return BadRequest(new { error = argEx.Message });
//            }
//            catch (Exception ex)
//            {
//                // Log the exception (using a logger, if available)
//                // _logger.LogError(ex, "An error occurred while adding movies.");
//                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred. Please try again later.", details = ex.Message });
//            }
//        }

//        [HttpPost("tvseries/update/{lastSeriesId}")]
//        public async Task<IActionResult> UpdateTVSeriesAsync(int lastSeriesId)
//        {
//            try
//            {
//                var id = await _tMDBMovieService.UpdateTVSeriesAndSeasonsEpisodesAsync(lastSeriesId);
//                return Ok(new { message = "TV Series Updated Successfully", lastUpdatedId = id });
//            }
//            catch (ArgumentException argEx)
//            {
//                // Handle specific argument errors
//                return BadRequest(new { error = argEx.Message });
//            }
//            catch (Exception ex)
//            {
//                // Log the exception if a logger is available
//                // _logger.LogError(ex, "An error occurred while updating TV series.");
//                return StatusCode(StatusCodes.Status500InternalServerError, new
//                {
//                    error = "An unexpected error occurred. Please try again later.",
//                    details = ex.Message
//                });
//            }
//        }


//    }
//}

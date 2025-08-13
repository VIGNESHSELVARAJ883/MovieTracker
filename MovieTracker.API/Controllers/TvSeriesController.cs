using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTracker.Data.Dtos;
using MovieTracker.Service;

namespace MovieTracker.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class TvSeriesController : Controller
    {
        private readonly ITvSeriesService _tvSeriesService;
        public TvSeriesController(ITvSeriesService tvSeriesService)
        {
            _tvSeriesService = tvSeriesService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTvSeriesAsync([FromQuery] TVSeriesFilterRequestDto filter)
        {
            var tvSeries = await _tvSeriesService.GetFilteredTVSeriesAsync(filter);
            return Ok(tvSeries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var series = await _tvSeriesService.GetTVSeriesDetailsAsync(id);
            if (series == null)
                return NotFound();

            return Ok(series);
        }
    }
}

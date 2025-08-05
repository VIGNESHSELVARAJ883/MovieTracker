using MovieTracker.Data.Dtos;

namespace MovieTracker.Service
{
    public interface ITvSeriesService
    {
        Task<IEnumerable<TVSeriesListDto>> GetFilteredTVSeriesAsync(TVSeriesFilterRequestDto filter);
        Task<TVSeriesDetailsDto?> GetTVSeriesDetailsAsync(int id);
    }
}

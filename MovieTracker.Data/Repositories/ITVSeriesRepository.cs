using MovieTracker.Data.Dtos;

namespace MovieTracker.Data.Repository
{
    public interface ITVSeriesRepository
    {
        Task<IEnumerable<TVSeriesListDto>> GetFilteredTVSeriesAsync(TVSeriesFilterRequestDto filter);
        Task<TVSeriesDetailsDto?> GetTVSeriesDetailsAsync(int id);
    }
}
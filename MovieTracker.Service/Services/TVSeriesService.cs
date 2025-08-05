using MovieTracker.Data.Dtos;
using MovieTracker.Data.Repository;

namespace MovieTracker.Service
{
    public class TvSeriesService : ITvSeriesService
    {
        private readonly ITVSeriesRepository _repository;

        public TvSeriesService(ITVSeriesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TVSeriesListDto>> GetFilteredTVSeriesAsync(TVSeriesFilterRequestDto filter)
        {
            return await _repository.GetFilteredTVSeriesAsync(filter);
        }

        public async Task<TVSeriesDetailsDto?> GetTVSeriesDetailsAsync(int id)
        {
            return await _repository.GetTVSeriesDetailsAsync(id);
        }
    }
}

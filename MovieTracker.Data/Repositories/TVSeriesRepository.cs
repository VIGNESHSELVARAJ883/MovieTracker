using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MovieTracker.Data.Dtos;
using MovieTracker.Data.Entities;
using System.Data;

namespace MovieTracker.Data.Repository
{
    public class TVSeriesRepository : ITVSeriesRepository
    {
        private readonly string _connectionString;

        public TVSeriesRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<IEnumerable<TVSeriesListDto>> GetFilteredTVSeriesAsync(TVSeriesFilterRequestDto filter)
        {
            using var conn = Connection;

            var parameters = new DynamicParameters();
            parameters.Add("@SearchTitle", filter.SearchTitle);
            parameters.Add("@FirstAirYear", filter.FirstAirYear);
            parameters.Add("@MinRating", filter.MinRating);
            parameters.Add("@MaxRating", filter.MaxRating);
            parameters.Add("@LanguageCode", filter.LanguageCode);
            parameters.Add("@MinEpisodeRuntime", filter.MinEpisodeRuntime);
            parameters.Add("@MaxEpisodeRuntime", filter.MaxEpisodeRuntime);
            parameters.Add("@MinPopularity", filter.MinPopularity);
            parameters.Add("@MaxPopularity", filter.MaxPopularity);
            parameters.Add("@GenreId", filter.GenreId);
            parameters.Add("@Status", filter.Status);
            parameters.Add("@SortColumn", filter.SortColumn);
            parameters.Add("@SortOrder", filter.SortOrder);
            parameters.Add("@PageNumber", filter.PageNumber);
            parameters.Add("@PageSize", filter.PageSize);

            var series = await conn.QueryAsync<TVSeriesListDto>(
                "sp_GetFilteredTVSeries", // You’ll need to create this SP similar to Movies
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return series;
        }



        public async Task<TVSeriesDetailsDto?> GetTVSeriesDetailsAsync(int id)
        {
            using var conn = Connection;

            using var multi = await conn.QueryMultipleAsync(
                "sp_GetTVSeriesDetails",
                new { TVSeriesId = id },
                commandType: CommandType.StoredProcedure
            );

            // First result: Series details
            var series = await multi.ReadFirstOrDefaultAsync<TVSeriesDetailsDto>();
            if (series == null) return null;

            // Second result: Seasons
            var seasons = (await multi.ReadAsync<SeasonDto>()).ToList();

            // Third result: Episodes
            var episodes = (await multi.ReadAsync<EpisodeDto>()).ToList();

            // Map episodes into their seasons
            foreach (var season in seasons)
            {
                season.Episodes = episodes.Where(e => e.SeasonId == season.SeasonId).ToList();
            }

            series.Seasons = seasons;

            return series;
        }
    }

}

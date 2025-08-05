using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MovieTracker.Data.Dtos;
using MovieTracker.Data.Entities;
using System.Data;

namespace MovieTracker.Data.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly string _connectionString;

        public MovieRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<IEnumerable<MovieListDto>> GetFilteredMoviesAsync(MovieFilterRequestDto filter)
        {
            using var conn = Connection;

            var parameters = new DynamicParameters();
            parameters.Add("@SearchTitle", filter.SearchTitle);
            parameters.Add("@ReleaseYear", filter.ReleaseYear);
            parameters.Add("@MinRating", filter.MinRating);
            parameters.Add("@MaxRating", filter.MaxRating);
            parameters.Add("@LanguageCode", filter.LanguageCode);
            parameters.Add("@MinRuntime", filter.MinRuntime);
            parameters.Add("@MaxRuntime", filter.MaxRuntime);
            parameters.Add("@MinPopularity", filter.MinPopularity);
            parameters.Add("@MaxPopularity", filter.MaxPopularity);
            parameters.Add("@GenreId", filter.GenreId);
            parameters.Add("@Status", filter.Status);
            parameters.Add("@SortColumn", filter.SortColumn);
            parameters.Add("@SortOrder", filter.SortOrder);
            parameters.Add("@PageNumber", filter.PageNumber);
            parameters.Add("@PageSize", filter.PageSize);

            var movies = await conn.QueryAsync<MovieListDto>(
                "sp_GetFilteredMovies",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return movies;
        }

        public async Task<IEnumerable<Language>> GetAllLanguagesAsync()
        {
            using var conn = Connection;
            return await conn.QueryAsync<Language>("SELECT Code, Name FROM Languages");
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            using var conn = Connection;
            return await conn.QueryAsync<Genre>("SELECT GenreId, GenreName FROM Genres");
        }
    }
}

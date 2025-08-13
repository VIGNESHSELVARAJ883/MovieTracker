using MovieTracker.Data.Dtos;
using MovieTracker.Data.Repositories;
using MovieTracker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker.Service.Services
{
    public class UserMovieService : IUserMovieService
    {
        private readonly IUserMovieRepository _repository;
        private readonly IMovieRepository _movieRepository;

        public UserMovieService(IUserMovieRepository repository, IMovieRepository movieRepository)
        {
            _repository = repository;
            _movieRepository = movieRepository;
        }

        public Task AddToWatchlistAsync(int userId, int movieId) =>
            _repository.AddToWatchlistAsync(userId, movieId);

        public Task RemoveFromWatchlistAsync(int userId, int movieId) =>
            _repository.RemoveFromWatchlistAsync(userId, movieId);

        public Task MarkAsWatchedAsync(int userId, int movieId) =>
            _repository.MarkAsWatchedAsync(userId, movieId);

        public Task RemoveFromWatchedAsync(int userId, int movieId) =>
            _repository.RemoveFromWatchedAsync(userId, movieId);

        public Task AddToFavouritesAsync(int userId, int movieId) =>
            _repository.AddToFavouritesAsync(userId, movieId);

        public Task RemoveFromFavouritesAsync(int userId, int movieId) =>
            _repository.RemoveFromFavouritesAsync(userId, movieId);

        public Task AddReviewAsync(int userId, int movieId, string review, decimal rating) =>
            _repository.AddReviewAsync(userId, movieId, review, rating);

        public Task RemoveReviewAsync(int userId, int movieId) =>
            _repository.RemoveReviewAsync(userId, movieId);

        public async Task<IEnumerable<MovieListDto>> GetWatchedMoviesAsync(int userId)
        {
            var filter = new MovieFilterRequestDto
            {
                UserId = userId,
                IsWatched = true
            };
            return await _movieRepository.GetFilteredMoviesAsync(filter);
        }

        public async Task<IEnumerable<MovieListDto>> GetFavouriteMoviesAsync(int userId)
        {
            var filter = new MovieFilterRequestDto
            {
                UserId = userId,
                IsFavourite = true
            };
            return await _movieRepository.GetFilteredMoviesAsync(filter);
        }

        public async Task<IEnumerable<MovieListDto>> GetToBeWatchedMoviesAsync(int userId)
        {
            var filter = new MovieFilterRequestDto
            {
                UserId = userId,
                IsWatchlist = true
            };
            return await _movieRepository.GetFilteredMoviesAsync(filter);
        }
    }

}

using MovieTracker.Data.Entities;

namespace MovieTracker.Data.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken token);
        Task<RefreshToken> GetByTokenAsync(string token);
        Task SaveChangesAsync();
    }
}

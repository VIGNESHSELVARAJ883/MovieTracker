using Microsoft.EntityFrameworkCore;
using MovieTracker.Data.Entities;

namespace MovieTracker.Data.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly MovieDbContext _context;
        public RefreshTokenRepository(MovieDbContext context) => _context = context;

        public async Task AddAsync(RefreshToken token) => await _context.RefreshTokens.AddAsync(token);

        public async Task<RefreshToken> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == token && r.RevokedAt == null);
        }


        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}

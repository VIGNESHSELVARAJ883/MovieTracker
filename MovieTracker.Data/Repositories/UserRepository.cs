using Microsoft.EntityFrameworkCore;
using MovieTracker.Data.Entities;

namespace MovieTracker.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MovieDbContext _context;
        public UserRepository(MovieDbContext context) => _context = context;

        public async Task<User> GetByEmailAsync(string email)
        {

            return await _context.Users
        .Include(s => s.RefreshTokens)
        .FirstOrDefaultAsync(u => u.Email == email);
        }


        public async Task<User> GetByIdAsync(int id) =>
            await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);

        public async Task AddAsync(User user) => await _context.Users.AddAsync(user);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}

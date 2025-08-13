using MovieTracker.Data.Dtos;
using MovieTracker.Data.Entities;
using MovieTracker.Data.Repositories;
using MovieTracker.Service.Utils;

namespace MovieTracker.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IRefreshTokenRepository _refreshRepo;
        private readonly IJwtGenerator _jwtGenerator;

        public AuthService(IUserRepository userRepo, IRefreshTokenRepository refreshRepo, IJwtGenerator jwtGenerator)
        {
            _userRepo = userRepo;
            _refreshRepo = refreshRepo;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
        {
            if (await _userRepo.GetByEmailAsync(dto.Email) != null)
                throw new Exception("Email already in use");

            CreatePasswordHash(dto.Password, out string hash, out string salt);

            var user = new User
            {
                Username = dto.UserName,
                Email = dto.Email,
                PasswordHash = hash, // stored as Base64 string
                PasswordSalt = salt,  // stored as Base64 string
                CreatedAt = DateTime.UtcNow,
            };

            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();

            var tokens = await GenerateTokensAsync(user);
            return tokens;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);
            if (user == null || !VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Invalid credentials");

            var tokens = await GenerateTokensAsync(user);
            return tokens;
        }

        public async Task<AuthResponseDto> RefreshAsync(string refreshToken)
        {
            var tokenEntity = await _refreshRepo.GetByTokenAsync(refreshToken);
            if (tokenEntity == null || tokenEntity.ExpiresAt < DateTime.UtcNow)
                throw new Exception("Invalid or expired refresh token");

            var tokens = await GenerateTokensAsync(tokenEntity.User);
            tokenEntity.RevokedAt = DateTime.UtcNow;
            await _refreshRepo.SaveChangesAsync();

            return tokens;
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var tokenEntity = await _refreshRepo.GetByTokenAsync(refreshToken);
            if (tokenEntity != null)
            {
                tokenEntity.RevokedAt = DateTime.UtcNow;
                await _refreshRepo.SaveChangesAsync();
            }
        }

        private async Task<AuthResponseDto> GenerateTokensAsync(User user)
        {
            var accessToken = _jwtGenerator.GenerateAccessToken(user);
            var refreshToken = _jwtGenerator.GenerateRefreshToken();

            var refreshEntity = new RefreshToken
            {
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                UserId = user.UserId
            };

            await _refreshRepo.AddAsync(refreshEntity);
            await _refreshRepo.SaveChangesAsync();

            return new AuthResponseDto { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        private void CreatePasswordHash(string password, out string hash, out string salt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            var saltBytes = hmac.Key;
            var hashBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            salt = Convert.ToBase64String(saltBytes);
            hash = Convert.ToBase64String(hashBytes);
        }

        private bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var hashBytes = Convert.FromBase64String(storedHash);

            using var hmac = new System.Security.Cryptography.HMACSHA512(saltBytes);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(hashBytes);
        }
    }
}

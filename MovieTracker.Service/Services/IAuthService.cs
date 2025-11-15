using MovieTracker.Data.Dtos;

namespace MovieTracker.Service.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
        Task<AuthResponseDto> RefreshAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
        //Task<UserProfileDto> GetProfileAsync(Guid userId);
    }

}

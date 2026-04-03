using Application.Features._auth.DTOs.Request;
using Application.Wrappers;
using Domain.Entities;
using Models.Response._auth;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<Response<ApplicationUser>> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken);
        Task<Response<LoginResponse>> LoginUserAsync(LoginRequest request, CancellationToken cancellationToken);
        Task<Response<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken);
        Task<Response<string>> UpgradeToPremiumAsync(string userId);
        Task<Response<bool>> LogoutAsync(string refreshToken);
    }
}

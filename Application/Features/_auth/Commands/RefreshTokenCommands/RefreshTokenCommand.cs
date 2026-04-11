using Application.Features._auth.DTOs.Request;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using Application.Features._auth.DTOs;

namespace Application.Features._auth.Commands.RefreshTokenCommands
{
    public record RefreshTokenCommand(RefreshTokenRequest Request) : IRequest<Response<LoginResponse>>;

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Response<LoginResponse>>
    {
        private readonly IAuthService _authService;

        public RefreshTokenCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Response<LoginResponse>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            return await _authService.RefreshTokenAsync(command.Request, cancellationToken);
        }
    }
}

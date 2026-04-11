using Application.Features._auth.DTOs;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;

namespace Application.Features._auth.Commands.RefreshTokenCommands
{
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

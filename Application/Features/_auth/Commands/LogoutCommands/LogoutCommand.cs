using Application.Interfaces;
using Application.Wrappers;
using MediatR;

namespace Application.Features._auth.Commands.LogoutCommands
{
    public record LogoutCommand(string RefreshToken) : IRequest<Response<bool>>;

    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Response<bool>>
    {
        private readonly IAuthService _authService;

        public LogoutCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Response<bool>> Handle(LogoutCommand command, CancellationToken cancellationToken)
        {
            return await _authService.LogoutAsync(command.RefreshToken);
        }
    }
}

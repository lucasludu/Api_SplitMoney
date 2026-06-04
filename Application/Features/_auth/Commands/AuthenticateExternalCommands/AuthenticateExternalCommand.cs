using Application.Features._auth.DTOs;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;

namespace Application.Features._auth.Commands.AuthenticateExternalCommands
{
    public record AuthenticateExternalCommand(ExternalAuthRequest Request) : IRequest<Response<LoginResponse>>;

    public class AuthenticateExternalCommandHandler : IRequestHandler<AuthenticateExternalCommand, Response<LoginResponse>>
    {
        private readonly IAuthService _authService;

        public AuthenticateExternalCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Response<LoginResponse>> Handle(AuthenticateExternalCommand request, CancellationToken cancellationToken)
        {
            return await _authService.AuthenticateExternalAsync(request.Request, cancellationToken);
        }
    }
}

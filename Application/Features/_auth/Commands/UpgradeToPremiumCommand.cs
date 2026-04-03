using Application.Interfaces;
using Application.Wrappers;
using MediatR;

namespace Application.Features._auth.Commands.UpgradeToPremium
{
    public record UpgradeToPremiumCommand : IRequest<Response<string>>;

    public class UpgradeToPremiumCommandHandler : IRequestHandler<UpgradeToPremiumCommand, Response<string>>
    {
        private readonly IAuthService _authService;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public UpgradeToPremiumCommandHandler(IAuthService authService, IAuthenticatedUserService authenticatedUser)
        {
            _authService = authService;
            _authenticatedUser = authenticatedUser;
        }

        public async Task<Response<string>> Handle(UpgradeToPremiumCommand request, CancellationToken cancellationToken)
        {
            return await _authService.UpgradeToPremiumAsync(_authenticatedUser.UserId);
        }
    }
}

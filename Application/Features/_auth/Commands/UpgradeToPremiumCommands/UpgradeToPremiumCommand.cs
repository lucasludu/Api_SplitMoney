using Application.Wrappers;
using MediatR;

namespace Application.Features._auth.Commands.UpgradeToPremiumCommands
{
    public record UpgradeToPremiumCommand : IRequest<Response<string>>;
}

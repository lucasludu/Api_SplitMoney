using Application.Wrappers;
using MediatR;

namespace Application.Features._auth.Commands.LogoutCommands
{
    public record LogoutCommand(string RefreshToken) : IRequest<Response<bool>>;
}

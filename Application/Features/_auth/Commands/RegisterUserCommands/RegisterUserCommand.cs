using Application.Features._auth.DTOs.Request;
using Application.Wrappers;
using MediatR;

namespace Application.Features._auth.Commands.RegisterUserCommands
{
    public record RegisterUserCommand(RegisterUserRequest Request) : IRequest<Response<string>>;
}

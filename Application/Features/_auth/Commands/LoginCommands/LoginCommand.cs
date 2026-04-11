using Application.Features._auth.DTOs;
using Application.Features._auth.DTOs.Request;
using Application.Wrappers;
using MediatR;

namespace Application.Features._auth.Commands.LoginCommands
{
    public record LoginCommand(LoginRequest Request) : IRequest<Response<LoginResponse>>;
}

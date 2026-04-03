using Application.Features._auth.DTOs.Request;
using Application.Wrappers;
using MediatR;
using Models.Response._auth;

namespace Application.Features._auth.Commands.LoginCommands
{
    public record LoginCommand(LoginRequest Request) : IRequest<Response<LoginResponse>>;
}

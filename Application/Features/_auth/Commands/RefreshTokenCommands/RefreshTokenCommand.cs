using Application.Features._auth.DTOs;
using Application.Features._auth.DTOs.Request;
using Application.Wrappers;
using MediatR;

namespace Application.Features._auth.Commands.RefreshTokenCommands
{
    public record RefreshTokenCommand(RefreshTokenRequest Request) : IRequest<Response<LoginResponse>>;
}

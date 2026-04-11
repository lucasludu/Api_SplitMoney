using Application.Features._user.DTOs;
using Application.Wrappers;
using MediatR;

namespace Application.Features._user.Queries.GetMyProfile
{
    public record GetMyProfileQuery : IRequest<Response<UserResponse>>;
}

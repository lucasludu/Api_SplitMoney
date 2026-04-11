using Application.Features._user.DTOs;
using Application.Wrappers;
using MediatR;

namespace Application.Features._user.Queries.SearchUsers
{
    public record SearchUsersQuery(string Term) : IRequest<Response<List<UserResponse>>>;
}

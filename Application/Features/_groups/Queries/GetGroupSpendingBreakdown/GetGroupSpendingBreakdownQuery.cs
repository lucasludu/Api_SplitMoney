using Application.Features._groups.DTOs;
using Application.Wrappers;
using MediatR;

namespace Application.Features.Groups.Queries
{
    public record GetGroupSpendingBreakdownQuery(Guid GroupId) : IRequest<Response<GroupSpendingBreakdownResponse>>;
}

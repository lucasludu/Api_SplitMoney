using Application.Wrappers;
using MediatR;
using Application.Features._expenses.DTOs;

namespace Application.Features.Expenses.Queries
{
    public record GetGroupBalanceQuery(Guid GroupId) : IRequest<Response<List<BalanceResponse>>>;
}

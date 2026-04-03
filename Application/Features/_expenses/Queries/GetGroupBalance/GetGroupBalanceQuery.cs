using Application.Wrappers;
using MediatR;
using Models.Response._balance;

namespace Application.Features.Expenses.Queries.GetGroupBalance
{
    public record GetGroupBalanceQuery(Guid GroupId) : IRequest<Response<List<BalanceResponse>>>;
}

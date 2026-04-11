using Application.Features._expenses.DTOs;
using Application.Wrappers;
using MediatR;

namespace Application.Features.Expenses.Queries
{
    public record GetExpenseDetailQuery(Guid Id) : IRequest<Response<ExpenseDetailResponse>>;
}

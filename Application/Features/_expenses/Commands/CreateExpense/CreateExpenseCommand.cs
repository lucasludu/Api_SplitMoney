using Application.Wrappers;
using MediatR;
using Application.Features._expenses.DTOs;

namespace Application.Features.Expenses.Commands.CreateExpense
{
    public record CreateExpenseCommand(ExpenseRequest Request) : IRequest<Response<Guid>>;
}

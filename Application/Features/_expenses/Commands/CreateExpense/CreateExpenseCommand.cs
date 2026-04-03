using Application.Wrappers;
using MediatR;
using Models.Request._expenses;

namespace Application.Features.Expenses.Commands.CreateExpense
{
    public record CreateExpenseCommand(ExpenseRequest Request) : IRequest<Response<Guid>>;
}

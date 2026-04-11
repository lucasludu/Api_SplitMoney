using Application.Wrappers;
using MediatR;

namespace Application.Features.Expenses.Commands.DeleteExpense
{
    public record DeleteExpenseCommand(Guid Id) : IRequest<Response<Guid>>;
}

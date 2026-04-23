using Application.Wrappers;
using MediatR;

namespace Application.Features.Expenses.Commands.ConfirmExpense
{
    public class ConfirmExpenseCommand : IRequest<Response<bool>>
    {
        public Guid ExpenseId { get; set; }
    }
}

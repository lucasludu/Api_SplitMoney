using Application.Features._expenses.DTOs;
using Application.Wrappers;
using MediatR;

namespace Application.Features.Expenses.Commands.UpdateExpense
{
    public record UpdateExpenseCommand(UpdateExpenseRequest Request) : IRequest<Response<Guid>>;

}

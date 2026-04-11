using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Features._expenses.DTOs;
using Application.Specification._expenses;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Common;

namespace Application.Features.Expenses.Commands.UpdateExpense
{
    public record UpdateExpenseCommand(UpdateExpenseRequest Request) : IRequest<Response<Guid>>;

    public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand, Response<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public UpdateExpenseCommandHandler(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUser)
        {
            _unitOfWork = unitOfWork;
            _authenticatedUser = authenticatedUser;
        }

        public async Task<Response<Guid>> Handle(UpdateExpenseCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var userId = _authenticatedUser.UserId;

            var spec = new ExpenseWithSplitsSpecification(request.Id);
            var expense = await _unitOfWork.RepositoryAsync<Expense>()
                .FirstOrDefaultAsync(spec, cancellationToken);

            if (expense == null) return Response<Guid>.Fail("Gasto no encontrado.");

            // Auditoría: Comparar campos y guardar cambios
            await LogChanges(expense, request);

            // Actualizar campos básicos
            expense.Title = request.Title;
            expense.Amount = new Money(request.TotalAmount, request.Currency);
            expense.Date = request.Date;
            expense.CategoryId = request.CategoryId;

            // Nota: En una implementación real más compleja, aquí actualizaríamos los Splits también.
            // Para redondear el flujo, nos enfocaremos en las auditorías de campos básicos.

            await _unitOfWork.RepositoryAsync<Expense>().UpdateAsync(expense);
            await _unitOfWork.SaveChangesAsync();

            return new Response<Guid>(expense.Id, "Gasto actualizado y auditado correctamente.");
        }

        private async Task LogChanges(Expense oldExpense, UpdateExpenseRequest newRequest)
        {
            var auditsRepo = _unitOfWork.RepositoryAsync<ExpenseAudit>();

            // Auditoría de Título
            if (oldExpense.Title != newRequest.Title)
            {
                await auditsRepo.AddAsync(new ExpenseAudit
                {
                    ExpenseId = oldExpense.Id,
                    Action = "Cambio de Título",
                    PreviousValue = oldExpense.Title,
                    NewValue = newRequest.Title,
                    ModifiedByUserId = _authenticatedUser.UserId
                });
            }

            // Auditoría de Monto
            if (oldExpense.Amount.Amount != newRequest.TotalAmount)
            {
                await auditsRepo.AddAsync(new ExpenseAudit
                {
                    ExpenseId = oldExpense.Id,
                    Action = "Cambio de Monto",
                    PreviousValue = $"{oldExpense.Amount.Amount:N2}",
                    NewValue = $"{newRequest.TotalAmount:N2}",
                    ModifiedByUserId = _authenticatedUser.UserId
                });
            }

            // Auditoría de Categoría
            if (oldExpense.CategoryId != newRequest.CategoryId)
            {
                await auditsRepo.AddAsync(new ExpenseAudit
                {
                    ExpenseId = oldExpense.Id,
                    Action = "Cambio de Categoría",
                    PreviousValue = oldExpense.CategoryId?.ToString() ?? "Sin Categoría",
                    NewValue = newRequest.CategoryId?.ToString() ?? "Sin Categoría",
                    ModifiedByUserId = _authenticatedUser.UserId
                });
            }
        }
    }
}

using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Features.Expenses.Commands.ConfirmExpense
{
    public class ConfirmExpenseCommandHandler : IRequestHandler<ConfirmExpenseCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public ConfirmExpenseCommandHandler(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUser)
        {
            _unitOfWork = unitOfWork;
            _authenticatedUser = authenticatedUser;
        }

        public async Task<Response<bool>> Handle(ConfirmExpenseCommand request, CancellationToken cancellationToken)
        {
            var expense = await _unitOfWork.RepositoryAsync<Expense>().GetByIdAsync(request.ExpenseId);
            if (expense == null) return new Response<bool>("Gasto no encontrado.");

            expense.IsConfirmed = true;
            await _unitOfWork.RepositoryAsync<Expense>().UpdateAsync(expense);

            // Marcar notificaciones relacionadas como leídas para este usuario
            // Nota: En una implementación real usaríamos una especificación para filtrar en BD
            var notifications = await _unitOfWork.RepositoryAsync<Notification>().ListAsync();
            var userNotifications = notifications.Where(n => n.UserId == _authenticatedUser.UserId && n.RelatedId == request.ExpenseId);
            
            foreach (var n in userNotifications)
            {
                n.IsRead = true;
                await _unitOfWork.RepositoryAsync<Notification>().UpdateAsync(n);
            }

            await _unitOfWork.SaveChangesAsync();
            return new Response<bool>(true, "Gasto confirmado correctamente.");
        }
    }
}

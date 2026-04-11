using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Features.Expenses.Commands.DeleteExpense
{
    public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand, Response<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public DeleteExpenseCommandHandler(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUser)
        {
            _unitOfWork = unitOfWork;
            _authenticatedUser = authenticatedUser;
        }

        public async Task<Response<Guid>> Handle(DeleteExpenseCommand command, CancellationToken cancellationToken)
        {
            var expense = await _unitOfWork.RepositoryAsync<Expense>().GetByIdAsync(command.Id, cancellationToken);

            if (expense == null) return Response<Guid>.Fail("El gasto no existe.");

            // Opcional: Validar que sea el dueño del gasto o el admin?
            // En este prototipo asumiremos que cualquiera del grupo podría reportar errores,
            // pero validemos al menos que el gasto pertenezca a un grupo del usuario?
            // Por simplicidad en esta fase, permitimos borrar si se conoce el GUID.

            await _unitOfWork.RepositoryAsync<Expense>().DeleteAsync(expense);
            await _unitOfWork.SaveChangesAsync();

            return new Response<Guid>(expense.Id, "Gasto eliminado correctamente.");
        }
    }
}

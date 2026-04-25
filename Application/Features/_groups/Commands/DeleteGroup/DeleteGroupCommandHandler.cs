using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Groups.Commands
{
    public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, Response<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public DeleteGroupCommandHandler(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUser)
        {
            _unitOfWork = unitOfWork;
            _authenticatedUser = authenticatedUser;
        }

        public async Task<Response<Guid>> Handle(DeleteGroupCommand command, CancellationToken cancellationToken)
        {
            var userId = _authenticatedUser.UserId;

            // 1. Obtener el grupo con sus miembros y gastos para validación y limpieza
            var group = await _unitOfWork.RepositoryAsync<Group>().Entities
                .Include(g => g.Members)
                .Include(g => g.Expenses)
                .ThenInclude(e => e.Splits)
                .Include(g => g.Expenses)
                .ThenInclude(e => e.Payments)
                .Include(g => g.Balances)
                .Include(g => g.Settlements)
                .FirstOrDefaultAsync(g => g.Id == command.Id, cancellationToken);

            if (group == null)
            {
                return Response<Guid>.Fail("El círculo no existe.");
            }

            // 2. Validar que el usuario que intenta eliminar sea miembro del grupo
            var userMember = group.Members.FirstOrDefault(m => m.UserId == userId);
            if (userMember == null)
            {
                return Response<Guid>.Fail("No tienes permisos para eliminar este círculo.");
            }

            // Opcional: Solo permitir a los administradores borrar el grupo
            if (!userMember.IsAdmin)
            {
                 return Response<Guid>.Fail("Solo los administradores del círculo pueden eliminarlo.");
            }

            // 3. Limpieza manual de entidades relacionadas (para evitar errores de FK en SQLite/SQL)
            // Borramos los detalles de los gastos primero
            foreach (var expense in group.Expenses.ToList())
            {
                if (expense.Splits != null)
                {
                    foreach (var split in expense.Splits.ToList())
                        await _unitOfWork.RepositoryAsync<ExpenseSplit>().DeleteAsync(split);
                }
                
                if (expense.Payments != null)
                {
                    foreach (var payment in expense.Payments.ToList())
                        await _unitOfWork.RepositoryAsync<ExpensePayment>().DeleteAsync(payment);
                }

                await _unitOfWork.RepositoryAsync<Expense>().DeleteAsync(expense);
            }

            // Borramos balances, liquidaciones y miembros
            foreach (var balance in group.Balances.ToList())
                await _unitOfWork.RepositoryAsync<Balance>().DeleteAsync(balance);

            foreach (var settlement in group.Settlements.ToList())
                await _unitOfWork.RepositoryAsync<Settlement>().DeleteAsync(settlement);

            foreach (var member in group.Members.ToList())
                await _unitOfWork.RepositoryAsync<GroupMember>().DeleteAsync(member);

            // 4. Finalmente borramos el grupo
            await _unitOfWork.RepositoryAsync<Group>().DeleteAsync(group);
            
            await _unitOfWork.SaveChangesAsync();

            return new Response<Guid>(group.Id, "Círculo eliminado y todas sus transacciones asociadas han sido borradas.");
        }
    }
}

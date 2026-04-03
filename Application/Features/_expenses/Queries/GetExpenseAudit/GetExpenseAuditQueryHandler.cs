using Application.Constants;
using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features._expenses.Queries.GetExpenseAudit
{
    public class GetExpenseAuditQueryHandler : IRequestHandler<GetExpenseAuditQuery, Response<ExpenseAuditResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public GetExpenseAuditQueryHandler(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUser)
        {
            _unitOfWork = unitOfWork;
            _authenticatedUser = authenticatedUser;
        }

        public async Task<Response<ExpenseAuditResponse>> Handle(GetExpenseAuditQuery request, CancellationToken cancellationToken)
        {
            // Verificación del rol en la capa de negocio
            if (!_authenticatedUser.Roles.Contains(RolesConstants.PremiumUser))
            {
                throw new ApiException("Esta característica es exclusiva para usuarios Premium.");
            }

            var expense = await _unitOfWork.RepositoryAsync<Expense>().GetByIdAsync(request.ExpenseId);
            if (expense == null) throw new ApiException("Gasto no encontrado.");

            var auditLogs = await _unitOfWork.RepositoryAsync<ExpenseAudit>()
                .Entities
                .Include(a => a.ModifiedByUser)
                .Where(a => a.ExpenseId == request.ExpenseId)
                .OrderByDescending(a => a.ChangeDate)
                .Select(a => new ExpenseAuditLogEntry
                {
                    Action = a.Action,
                    PreviousValue = a.PreviousValue,
                    NewValue = a.NewValue,
                    ModifiedBy = $"{a.ModifiedByUser.FirstName} {a.ModifiedByUser.LastName}",
                    ChangeDate = a.ChangeDate
                })
                .ToListAsync(cancellationToken);

            return new Response<ExpenseAuditResponse>(new ExpenseAuditResponse
            {
                ExpenseId = expense.Id,
                History = auditLogs
            });
        }
    }
}

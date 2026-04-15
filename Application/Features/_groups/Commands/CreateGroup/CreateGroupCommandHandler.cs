using Application.Constants;
using Application.Exceptions;
using Application.Interfaces;
using Application.Specification._user;
using Application.Wrappers;
using Domain.Entities;
using Domain.Common;
using MediatR;

namespace Application.Features.Groups.Commands
{
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Response<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticatedUserService _authenticatedUser;
        private readonly IAuthService _authService;

        public CreateGroupCommandHandler(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUser, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authenticatedUser = authenticatedUser;
            _authService = authService;
        }

        public async Task<Response<Guid>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            var userId = _authenticatedUser.UserId;
            var isPremium = _authenticatedUser.Roles.Contains(RolesConstants.PremiumUser);

            // Regla de Negocio: Límite de grupos para usuarios Free
            if (!isPremium)
            {
                var spec = new ActiveGroupsByUserCountSpecification(userId);
                var activeGroupsCount = await _unitOfWork.RepositoryAsync<GroupMember>().CountAsync(spec, cancellationToken);
                if (activeGroupsCount >= 3)
                    return Response<Guid>.Fail("Has alcanzado el límite de grupos activos para usuarios Free. Considera actualizar a Premium para crear más grupos.");

                if (request.InitialMembers.Count + 1 > 5)
                    return Response<Guid>.Fail("Has alcanzado el límite de miembros por grupo para usuarios Free. Considera actualizar a Premium para agregar más miembros.");
            }

            // Resolve emails to actual User IDs and build members list
            var memberDict = new Dictionary<string, string>(); // Email -> UserID
            
            // Add creator
            var creator = await _unitOfWork.RepositoryAsync<ApplicationUser>().GetByIdAsync(userId);
            if (creator == null) 
                return Response<Guid>.Fail("Usuario creador no encontrado.");
           
            memberDict[creator.Email!] = userId;
            
            foreach (var record in request.InitialMembers)
            {
                if (record.Email.Equals(creator.Email, StringComparison.OrdinalIgnoreCase)) continue;
                
                var user = await _authService.GetOrCreatePlaceholderUserAsync(record.Email, record.FullName);
                memberDict[record.Email] = user.Id;
            }

            // Create Group instance
            var group = new Group
            {
                Name = request.Name,
                Description = request.Description,
                DefaultCurrency = request.DefaultCurrency
            };

            // Add all resolved users to the group
            foreach (var entry in memberDict)
            {
                group.Members.Add(new GroupMember
                {
                    UserId = entry.Value,
                    JoinedAt = DateTime.UtcNow,
                    IsAdmin = entry.Value == userId
                });
            }

            var newGroup = await _unitOfWork.RepositoryAsync<Group>().AddAsync(group);
            await _unitOfWork.SaveChangesAsync();

            // Create Initial Expenses for those who spent money
            foreach (var record in request.InitialMembers)
            {
                if (record.AmountSpent <= 0) continue;
                if (!memberDict.TryGetValue(record.Email, out var payerId)) continue;

                var expense = new Expense
                {
                    Title = $"Gasto inicial: {record.Email}",
                    Amount = new Money(record.AmountSpent, request.DefaultCurrency),
                    GroupId = newGroup.Id
                };

                expense.Payments.Add(new ExpensePayment
                {
                    UserId = payerId,
                    AmountPaid = record.AmountSpent
                });

                // Split equally among all group members
                decimal splitAmount = record.AmountSpent / memberDict.Count;
                foreach (var memberEntry in memberDict)
                {
                    expense.Splits.Add(new ExpenseSplit
                    {
                        UserId = memberEntry.Value,
                        AmountOwed = splitAmount
                    });
                }

                await _unitOfWork.RepositoryAsync<Expense>().AddAsync(expense);
            }

            await _unitOfWork.SaveChangesAsync();

            return new Response<Guid>(newGroup.Id, "Grupo y saldos iniciales creados correctamente.");
        }
    }
}

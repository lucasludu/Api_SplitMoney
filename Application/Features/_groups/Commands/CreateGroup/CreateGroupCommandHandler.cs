using Application.Constants;
using Application.Exceptions;
using Application.Interfaces;
using Application.Specification._user;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Features._groups.Commands.CreateGroup
{
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Response<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public CreateGroupCommandHandler(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUser)
        {
            _unitOfWork = unitOfWork;
            _authenticatedUser = authenticatedUser;
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
                {
                    throw new PremiumLimitException("grupos activos", 3);
                }

                // Regla de Negocio: Límite de miembros por grupo para usuarios Free
                if (request.MemberIds.Count + 1 > 5) // +1 por el creador
                {
                    throw new PremiumLimitException("miembros por grupo", 5);
                }
            }

            var group = new Group
            {
                Name = request.Name,
                Description = request.Description,
                DefaultCurrency = request.DefaultCurrency
            };

            // El creador es el administrador
            group.Members.Add(new GroupMember
            {
                UserId = userId,
                JoinedAt = DateTime.UtcNow,
                IsAdmin = true
            });

            // Agregar otros miembros
            foreach (var memberId in request.MemberIds)
            {
                group.Members.Add(new GroupMember
                {
                    UserId = memberId,
                    JoinedAt = DateTime.UtcNow,
                    IsAdmin = false
                });
            }

            var newGroup = await _unitOfWork.RepositoryAsync<Group>().AddAsync(group);
            await _unitOfWork.SaveChangesAsync();

            return new Response<Guid>(newGroup.Id, "Grupo creado correctamente.");
        }
    }
}

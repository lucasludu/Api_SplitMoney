using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features._notifications.Commands.DeleteNotification
{
    public class DeleteNotificationCommand : IRequest<Response<bool>>
    {
        public Guid? Id { get; set; }
        public bool ClearAll { get; set; }
    }

    public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public DeleteNotificationCommandHandler(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUser)
        {
            _unitOfWork = unitOfWork;
            _authenticatedUser = authenticatedUser;
        }

        public async Task<Response<bool>> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            var userId = _authenticatedUser.UserId;

            if (request.ClearAll)
            {
                var userNotifications = await _unitOfWork.RepositoryAsync<Notification>()
                    .Entities
                    .Where(n => n.UserId == userId)
                    .ToListAsync(cancellationToken);

                foreach (var n in userNotifications)
                {
                    await _unitOfWork.RepositoryAsync<Notification>().DeleteAsync(n);
                }
                
                await _unitOfWork.SaveChangesAsync();
                return new Response<bool>(true, "Todas las notificaciones han sido eliminadas.");
            }

            if (request.Id.HasValue)
            {
                var notification = await _unitOfWork.RepositoryAsync<Notification>().GetByIdAsync(request.Id.Value);
                if (notification == null || notification.UserId != userId)
                {
                    return Response<bool>.Fail("Notificación no encontrada.");
                }

                await _unitOfWork.RepositoryAsync<Notification>().DeleteAsync(notification);
                await _unitOfWork.SaveChangesAsync();
                return new Response<bool>(true, "Notificación eliminada.");
            }

            return Response<bool>.Fail("Solicitud inválida.");
        }
    }
}

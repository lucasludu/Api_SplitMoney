using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Features._notifications.DTOs;

namespace Application.Features._notifications.Queries.GetMyNotifications
{
    public class GetMyNotificationsHandler : IRequestHandler<GetMyNotificationsQuery, Response<List<NotificationResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public GetMyNotificationsHandler(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUser)
        {
            _unitOfWork = unitOfWork;
            _authenticatedUser = authenticatedUser;
        }

        public async Task<Response<List<NotificationResponse>>> Handle(GetMyNotificationsQuery request, CancellationToken cancellationToken)
        {
            var notifications = await _unitOfWork.RepositoryAsync<Notification>()
                .Entities
                .Where(n => n.UserId == _authenticatedUser.UserId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NotificationResponse
                {
                    Id = n.Id,
                    Message = n.Message,
                    Type = n.Type,
                    RelatedId = n.RelatedId,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return new Response<List<NotificationResponse>>(notifications);
        }
    }
}

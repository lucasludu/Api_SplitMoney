using Application.Wrappers;
using MediatR;
using Application.Features._notifications.DTOs;

namespace Application.Features._notifications.Queries.GetMyNotifications
{
    public class GetMyNotificationsQuery : IRequest<Response<List<NotificationResponse>>>
    {
    }
}

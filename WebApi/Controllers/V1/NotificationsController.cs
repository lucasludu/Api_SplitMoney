using Application.Features._notifications.Queries.GetMyNotifications;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    public class NotificationsController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetMyNotificationsQuery()));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await Mediator.Send(new Application.Features._notifications.Commands.DeleteNotification.DeleteNotificationCommand { Id = id }));
        }

        [HttpDelete("clear-all")]
        public async Task<IActionResult> ClearAll()
        {
            return Ok(await Mediator.Send(new Application.Features._notifications.Commands.DeleteNotification.DeleteNotificationCommand { ClearAll = true }));
        }
    }
}

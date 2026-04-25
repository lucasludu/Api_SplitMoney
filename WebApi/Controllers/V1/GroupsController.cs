using Application.Interfaces;
using Application.Features.Groups.Queries;
using Application.Features.Groups.Commands;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    public class GroupsController : BaseApiController
    {
        private readonly IAuthenticatedUserService _userService;

        public GroupsController(IAuthenticatedUserService userService) 
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetGroupsByUserQuery { UserId = _userService.UserId }));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(CreateGroupCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("{groupId:guid}/members")]
        [Authorize]
        public async Task<IActionResult> GetMembers(Guid groupId)
        {
            return Ok(await Mediator.Send(new GetGroupMembersQuery { GroupId = groupId }));
        }

        [HttpGet("{groupId:guid}/breakdown")]
        [Authorize]
        public async Task<IActionResult> GetBreakdown(Guid groupId)
        {
            return Ok(await Mediator.Send(new GetGroupSpendingBreakdownQuery(groupId)));
        }

        [HttpPost("settle")]
        [Authorize]
        public async Task<IActionResult> PostSettle(CreateSettlementCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{groupId:guid}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid groupId)
        {
            return Ok(await Mediator.Send(new DeleteGroupCommand(groupId)));
        }
    }
}

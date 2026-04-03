using Application.Interfaces;
using Application.Features._groups.Queries.GetGroupsByUser;
using Application.Features._groups.Commands.CreateGroup;
using Application.Features._groups.Commands.CreateSettlement;
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

        [HttpPost("settle")]
        [Authorize]
        public async Task<IActionResult> PostSettle(CreateSettlementCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}

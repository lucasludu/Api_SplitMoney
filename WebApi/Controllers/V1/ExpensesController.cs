using Application.Constants;
using Application.Features.Expenses.Commands.CreateExpense;
using Application.Features.Expenses.Queries;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Features._expenses.DTOs;

namespace WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    public class ExpensesController : BaseApiController
    {
        private readonly IAuthenticatedUserService _userService;

        public ExpensesController(IAuthenticatedUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await Mediator.Send(new GetExpenseDetailQuery(id)));
        }

        [HttpGet("dashboard")]
        [Authorize]
        public async Task<IActionResult> GetDashboard()
        {
            return Ok(await Mediator.Send(new GetUserDashboardQuery { UserId = _userService.UserId }));
        }
    
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(ExpenseRequest request)
        {
            var command = new CreateExpenseCommand(request);
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Put(Guid id, UpdateExpenseRequest request)
        {
            if (id != request.Id) return BadRequest();
            return Ok(await Mediator.Send(new Application.Features.Expenses.Commands.UpdateExpense.UpdateExpenseCommand(request)));
        }

        [HttpGet("{groupId:guid}/balances")]
        [Authorize]
        public async Task<IActionResult> GetBalances(Guid groupId)
        {
            return Ok(await Mediator.Send(new GetGroupBalanceQuery(groupId)));
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await Mediator.Send(new Application.Features.Expenses.Commands.DeleteExpense.DeleteExpenseCommand(id)));
        }

        [HttpGet("{id:guid}/audit")]
        [Authorize] /* 🧪 Bypass Premium check for dev: [Authorize(Roles = RolesConstants.PremiumUser)] */
        public async Task<IActionResult> GetAudit(Guid id)
        {
            return Ok(await Mediator.Send(new GetExpenseAuditQuery { ExpenseId = id }));
        }

        [HttpGet("groups/{groupId:guid}/summary")]
        [Authorize] /* 🧪 Bypass Premium check for dev: [Authorize(Roles = RolesConstants.PremiumUser)] */
        public async Task<IActionResult> GetSummary(Guid groupId)
        {
            return Ok(await Mediator.Send(new Application.Features.Expenses.Queries.GetSpendingSummary.GetGroupSpendingSummaryQuery { GroupId = groupId }));
        }

        [HttpGet("groups/{groupId:guid}/export")]
        [Authorize] /* 🧪 Bypass Premium check for dev: [Authorize(Roles = RolesConstants.PremiumUser)] */
        public async Task<IActionResult> GetExport(Guid groupId)
        {
            var result = await Mediator.Send(new Application.Features.Expenses.Queries.GetGroupExport.GetGroupExportQuery { GroupId = groupId });
            return File(result.Content, result.ContentType, result.FileName);
        }
    }
}

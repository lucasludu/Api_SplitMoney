using Application.Constants;
using Application.Features.Expenses.Commands.CreateExpense;
using Application.Features.Expenses.Queries.GetGroupBalance;
using Application.Features._expenses.Queries.GetExpenseAudit;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Request._expenses;

namespace WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    public class ExpensesController : BaseApiController
    {
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

        [HttpGet("{id:guid}/audit")]
        [Authorize(Roles = RolesConstants.PremiumUser)]
        public async Task<IActionResult> GetAudit(Guid id)
        {
            return Ok(await Mediator.Send(new GetExpenseAuditQuery { ExpenseId = id }));
        }
    }
}

using Application.Constants;
using Application.Features._categories.Commands.CreateCustomCategory;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    public class CategoriesController : BaseApiController
    {
        [HttpPost("custom")]
        [Authorize(Roles = RolesConstants.PremiumUser)]
        public async Task<IActionResult> CreateCustom(CreateCustomCategoryCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}

using Application.Features._auth.Commands.UpgradeToPremium;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    public class SubscriptionsController : BaseApiController
    {
        /// <summary>
        /// Sube el nivel de la cuenta del usuario autenticado a Premium.
        /// </summary>
        /// <returns>Resultado del Upgrade.</returns>
        [HttpPost("upgrade")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpgradeToPremium()
        {
            return Ok(await Mediator.Send(new UpgradeToPremiumCommand()));
        }

        /// <summary>
        /// Verifica el estado actual de la suscripción del usuario.
        /// </summary>
        /// <returns>Estado de la suscripción.</returns>
        [HttpGet("status")]
        [Authorize]
        public async Task<IActionResult> GetStatus()
        {
            // Este endpoint podría devolver más detalles, como fecha de vencimiento.
            return Ok(new { IsPremium = User.IsInRole("PremiumUser") });
        }
    }
}

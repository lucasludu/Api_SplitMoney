using Application.Features._user.Queries.GetMyProfile;
using Application.Features._user.Queries.SearchUsers;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class UserController : BaseApiController
    {
        /// <summary>
        /// Obtiene el perfil del usuario autenticado (basado en el token)
        /// </summary>
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            return Ok(await Mediator.Send(new GetMyProfileQuery()));
        }

        /// <summary>
        /// Busca usuarios por nombre o email para agregar a grupos
        /// </summary>
        /// <param name="term">Término de búsqueda</param>
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            return Ok(await Mediator.Send(new SearchUsersQuery(term)));
        }

        /// <summary>
        /// Obtiene todos los usuarios (Solo para Administradores)
        /// </summary>
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            // Reutilizamos el buscador sin términos para traer los primeros
            return Ok(await Mediator.Send(new SearchUsersQuery(string.Empty)));
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Renty.Application.Commands;
using System.Security.Claims;

namespace Renty.Web.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly IMediator _mediator;

        public FavoritesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// AJAX-метод для переключения статуса избранного (добавить/удалить)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFavorite([FromBody] string slug)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
            {
                return Unauthorized(new { success = false, message = "User not logged in" });
            }

            var userId = Guid.Parse(userIdStr);

            var result = await _mediator.Send(new ToggleFavoriteCommand(userId, slug));

            if (!result.IsSuccess)
            {
                return BadRequest(new { success = false, message = string.Join(", ", result.Errors) });
            }

            return Ok(new { success = true, isFavorite = result.Data });
        }
    }
}

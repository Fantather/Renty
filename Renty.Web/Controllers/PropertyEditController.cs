using MediatR;
using Microsoft.AspNetCore.Mvc;
using Renty.Application.Queries.Property;
using System.Security.Claims;

namespace Renty.Web.Controllers
{
    [Route("edit-property/{id:guid}")]
    public class PropertyEditController : Controller
    {
        private readonly IMediator _mediator;

        public PropertyEditController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid id)
        {
            var result = await _mediator.Send(new GetPropertyDraftQuery(id, CurrentUserId()));

            if (!result.IsSuccess)
                return NotFound();

            return RedirectToAction(nameof(Title), new { id });
        }

        [HttpGet("title")]
        public async Task<IActionResult> Title(Guid id)
        {
            var result = await _mediator.Send(new GetPropertyDraftQuery(id, CurrentUserId()));

            if (!result.IsSuccess)
                return NotFound();

            return View();
        }

        private Guid CurrentUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}

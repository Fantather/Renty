using MediatR;
using Microsoft.AspNetCore.Mvc;
using Renty.Application.Queries.Property;
using System.Security.Claims;


namespace Renty.Web.Controllers.Api
{
    [Route("api/map")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MapController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //Леше. По плану запрос выглядит примерно так:
        // GET: /api/map/properties?north=...&south=...&east=...&west=...&destination=Kyiv
        [HttpGet("properties")]
        public async Task<IActionResult> GetPropertiesInBounds([FromQuery] GetPropertiesByMapQuery query)
        {
            // айди текущего пользователя, если есть
            var currentUserIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(currentUserIdClaim, out var userId))
            {
                query = query with { UserId = userId };
            }


            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Errors });

            // данные для карты
            return Ok(result.Data);
        }
    }
}

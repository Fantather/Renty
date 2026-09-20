using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Renty.Application.Queries.Autocomplete;
using Renty.Domain.ServiceModels.Places;

namespace Renty.Web.Controllers.Api
{
    [Route("api/places")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PlacesController(IMediator mediator)
        {
            _mediator = mediator;   
        }

        [HttpGet("autocomplete")]
        public async Task<IActionResult> AutoComplete([FromQuery]string input, [FromQuery]string sessionToken)
        {
            if (string.IsNullOrEmpty(input))
                return Ok(new List<AddressSuggestionDto>());
            var result = await _mediator.Send(new AutocompleteQuery(input, sessionToken));

            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Errors);
        }
    }
}

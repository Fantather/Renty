using MediatR;
using Microsoft.AspNetCore.Http;
using Renty.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.CategoryCommands
{
    public record CreateCategoryCommand(string WebRootPath ,string Name, string? Description, IFormFile? ImageFile = null) : IRequest<OperationResult<Unit>>;
}

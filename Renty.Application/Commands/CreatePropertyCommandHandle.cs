using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.CreateProperty;

namespace Renty.Application.Commands
{
    public class CreatePropertyCommand : IRequest<OperationResult<Guid>>
    {
        public CreatePropertyDto Data { get; set; } = null!;
    }
}

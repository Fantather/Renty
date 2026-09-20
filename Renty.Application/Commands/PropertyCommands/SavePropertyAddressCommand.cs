using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.CreateProperty;

namespace Renty.Application.Commands.PropertyCommands
{
    public record SavePropertyAddressCommand(SavePropertyAddressDto Data) : IRequest<OperationResult<Guid>>;
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.RegisterCommands
{
    public record SendConfirmationEmailCommand(string Email, string ConfirmEmailBaseUrl) : IRequest;
}

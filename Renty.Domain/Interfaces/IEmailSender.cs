using Renty.Domain.ServiceModels;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Renty.Domain.Interfaces
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(EmailMessage message);
    }
}

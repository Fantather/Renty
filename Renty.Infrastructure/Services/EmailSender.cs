using MimeKit;
using MailKit.Net.Smtp;
using Renty.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Renty.Domain.ServiceModels;
using Microsoft.Extensions.Options;

namespace Renty.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailSender(IOptions<EmailConfiguration> options)
        {
            _emailConfig = options.Value;
        }
        public async Task<bool> SendEmailAsync( EmailMessage message)
        {
            var emailMessage = await CreateEmailMessageAsync(message);
            return await SendAsync(emailMessage);
        }
        private async Task<MimeMessage> CreateEmailMessageAsync(EmailMessage message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.To.Add(message.To.First());
            mimeMessage.From.Add(new MailboxAddress(_emailConfig.From, _emailConfig.From));
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return mimeMessage;
        }
        private async Task<bool> SendAsync(MimeMessage mimeMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    //Указываем smtp сервер почты и порт
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    //Указываем свой Email адрес и пароль приложения
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

                    await client.SendAsync(mimeMessage);
                    return true;
                }
                catch
                {

                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }

            }
            return false;
        }

    }
}

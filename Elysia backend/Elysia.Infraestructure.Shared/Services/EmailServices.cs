using Elysia.Core.Application.Dtos.Email;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;


namespace Elysia.Infraestructure.Shared.Services
{
    public class EmailServices : IEmailServices
    {

        public MailSettings MailSettings { get; }
        public readonly ILogger<EmailServices> _logger;


        public EmailServices(IOptions<MailSettings> mailSettings, ILogger<EmailServices> logger)
        {

            this.MailSettings = mailSettings.Value;
            _logger = logger;

        }





        public async Task<bool> SendAsync(EmailDto? dto)
        {
            try
            {


                dto.ToRange.Add(dto.To ?? "");


                MimeMessage email = new()
                {

                    Sender = MailboxAddress.Parse(MailSettings.EmailFrom),
                    Subject = dto.Subject


                };



                foreach (var toItem in dto.ToRange ?? [])
                {
                    email.To.Add(MailboxAddress.Parse(toItem));
                }



                BodyBuilder builder = new()
                {
                    HtmlBody = dto.HtmlBody
                };


                email.Body = builder.ToMessageBody();


                using MailKit.Net.Smtp.SmtpClient smptClient = new();
                await smptClient.ConnectAsync(MailSettings.SmtpHost, MailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await smptClient.AuthenticateAsync(MailSettings.SmtpUser, MailSettings.SmtpPass);
                await smptClient.SendAsync(email);
                await smptClient.DisconnectAsync(true);
                return true;

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while sending email");
                return false;

            }

        }







    }

}

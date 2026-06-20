
namespace Elysia.Core.Domain.Settings
{
    public class MailSettings
    {

        public required string EmailFrom { get; set; } = string.Empty;
        public required string SmtpHost { get; set; } = string.Empty;
        public   int SmtpPort { get; set; }
        public required string SmtpUser { get; set; } = string.Empty;
        public required  string SmtpPass { get; set; } = string.Empty;
        public required string DisplayName { get; set; } = string.Empty;


    }

}

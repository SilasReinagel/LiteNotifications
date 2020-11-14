using LiteNotifications.WebApi._Common;
using LiteNotifications.WebApi.Email;

namespace LiteNotifications.WebApi
{
    public sealed class EnvironmentVariablesConfig : IEmailSettings
    {
        public EmailSender EmailSender => new EmailSender(new EnvironmentVariable("NotificationsEmailSender"));
        public SmtpSettings SmtpSettings => new SmtpSettings(new EnvironmentVariable("NotificationsEmailSmtp"));
    }
}

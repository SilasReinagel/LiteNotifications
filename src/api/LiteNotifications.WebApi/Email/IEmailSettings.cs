namespace LiteNotifications.WebApi.Email
{
    public interface IEmailSettings
    {
        EmailSender EmailSender { get; }
        SmtpSettings SmtpSettings { get; }
    }
}

using System.Net.Mail;

namespace LiteNotifications.WebApi.Email
{
    public sealed class EmailClient
    {
        private readonly EmailSender _sender;
        private readonly SmtpClient _smtpClient;

        public EmailClient(IEmailSettings settings)
            : this(settings.EmailSender, settings.SmtpSettings) { }
        
        public EmailClient(EmailSender sender, SmtpSettings smtp)
        {
            _sender = sender;
            _smtpClient = new SmtpClient(smtp.Address) { Port = smtp.Port };
        }
        
        public void Send(MailMessage message)
        {
            message.From = new MailAddress(_sender.Address);
            _smtpClient.Send(message);
        }
    }
}

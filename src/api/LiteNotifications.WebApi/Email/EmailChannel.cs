using System.Net.Mail;
using System.Threading.Tasks;
using LiteNotifications.WebApi.Domain;

namespace LiteNotifications.WebApi.Email
{
    public sealed class EmailChannel : IChannel
    {
        private readonly EmailClient _client;
        
        public string Type => "Email";
        public EmailChannel(EmailClient client) => _client = client;

        public Task Send(RoutedNotification n)
        {
            var msg = new MailMessage();
            msg.To.Add(n.Target);
            msg.Subject = n.Title;
            msg.Body = n.Text;
            _client.Send(msg);
            return Task.CompletedTask;
        }
    }
}

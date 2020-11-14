
namespace LiteNotifications.WebApi.Domain
{
    public sealed class Notification
    {
        public string Topic { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
